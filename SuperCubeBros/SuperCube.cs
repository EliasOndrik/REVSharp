using REVSharp;
using Silk.NET.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using System.Drawing;
using REVSharp.ModelLoader;
using REVSharp.Core;
using REVSharp.Components;
using REVSharp.Behaviours;
using REVSharp.Input;

namespace SuperCubeBros
{
    internal class SuperCube : Game
    {
        private IServiceProvider _provider;
        private IEntityComponentSystem _ecs;
        private WindowOptions _windowOptions;
        private Entity _cube;
        private Entity _camera;
        private Entity _floor;
        private Entity[] _platforms; 
        private Entity _testCube;
        private IInputManager _inputManager;
        public SuperCube(WindowOptions windowOptions) : base(windowOptions)
        {
            _windowOptions = windowOptions;
            _platforms = new Entity[40];
        }

        protected override void Load()
        {

            IServiceCollection services = new ServiceCollection();
            InitializeDependencies(ref services);
            services.AddSingleton<ThirdPersonCameraSystem>();
            services.AddSingleton<Colision>();
            services.AddSingleton<PlayerControl>();

            _provider = services.BuildServiceProvider();
            InitializeECS(ref _provider);
            _provider.GetRequiredService<GL>().ClearColor(Color.DeepSkyBlue);
            IModelManager modelManager = _provider.GetRequiredService<IModelManager>();
            modelManager.LoadModel("cube.obj");
            
            _ecs = _provider.GetRequiredService<IEntityComponentSystem>();
            _ecs.RegisterComponent<ColisionBox>();
            _ecs.RegisterComponent<PlayerComponent>();

            ThirdPersonCameraSystem cameraSystem = _provider.GetRequiredService<ThirdPersonCameraSystem>();
            _ecs.RegisterSystem(cameraSystem);
            _ecs.SetSystemMask<ThirdPersonCameraSystem>(_ecs.GetComponentMask<Camera>() | _ecs.GetComponentMask<Transform>());
            cameraSystem.SetScreenSize(_windowOptions.Size.X, _windowOptions.Size.Y);

            Colision colisionSystem = _provider.GetRequiredService<Colision>();
            _ecs.RegisterSystem(colisionSystem);
            _ecs.SetSystemMask<Colision>(_ecs.GetComponentMask<Transform>() | _ecs.GetComponentMask<ColisionBox>());

            PlayerControl playerControl = _provider.GetRequiredService<PlayerControl>();
            _ecs.RegisterSystem(playerControl);
            _ecs.SetSystemMask<PlayerControl>(_ecs.GetComponentMask<Transform>() | _ecs.GetComponentMask<PlayerComponent>() | _ecs.GetComponentMask<ColisionBox>());

            


            modelManager.CreateColisionBox("cube.obj", out Vector3D<float> leftBottomBack, out Vector3D<float> rightTopFront);
            SimpleColisionBox colisionBox = new()
            {
                Left = leftBottomBack.X,
                Right = rightTopFront.X,
                Bottom = leftBottomBack.Y,
                Top = rightTopFront.Y,
                Back = leftBottomBack.Z,
                Front = rightTopFront.Z
            };
            _camera = _ecs.CreateEntity();
            _ecs.AddComponent(ref _camera, Transform.Default with
            {
                Position = new Vector3D<float>(0, 0, 10f)
            });
            _ecs.AddComponent(ref _camera, Camera.ThirdPersonCamera with
            {
                Distance = 15f
            });
            _provider.GetRequiredService<Render>().SetCamera(_camera);

            _cube = _ecs.CreateEntity();
            _ecs.AddComponent(ref _cube, Transform.Default with
            {
                Position = new Vector3D<float>(0,3,-40)
            });
            _ecs.AddComponent(ref _cube, ModelId.Default with
            {
                ModelIndex = modelManager.GetModelIndex("cube.obj"),
                Color = new Vector3D<float>(1, 0, 0)
            });
            _ecs.AddComponent(ref _cube, ColisionBox.Default with
            {
                Box = colisionBox,
                Type = ColisionType.Dynamic
            });
            _ecs.AddComponent(ref _cube, new PlayerComponent());
            cameraSystem.SetTarget(_cube);

            _inputManager = _provider.GetRequiredService<IInputManager>();
            int cubeId = modelManager.GetModelIndex("cube.obj");
            //_floor = _ecs.CreateEntity();
            //_ecs.AddComponent(ref _floor, Transform.Default with
            //{
            //    Scale = new Vector3D<float>(100, 2, 100)
            //});
            //_ecs.AddComponent(ref _floor, ModelId.Default with
            //{
            //    ModelIndex = modelManager.GetModelIndex("cube.obj")
            //});
            //_ecs.AddComponent(ref _floor, ColisionBox.Default with
            //{
            //    Box = colisionBox,
            //});


            using (StreamReader level = File.OpenText("level1.csv"))
            {
                int i = 0;
                while (!level.EndOfStream)
                {
                    string line = level.ReadLine()??"";
                    if (line == "")
                    {
                        continue;
                    }
                    string[] stingInfo = line.Split(';');
                    int[] intInfo = Array.ConvertAll(stingInfo, s => int.Parse(s));
                    Vector3D<float> position = new(intInfo[0], intInfo[1], intInfo[2]);
                    Vector3D<float> scale = new(intInfo[3], intInfo[4], intInfo[5]);
                    Vector3D<float> color = new(intInfo[6], intInfo[7], intInfo[8]);
                    color /= 255.0f;
                    _platforms[i] = _ecs.CreateEntity();
                    _ecs.AddComponent(ref _platforms[i], Transform.Default with
                    {
                        Position = position,
                        Scale = scale
                    });
                    _ecs.AddComponent(ref _platforms[i], ModelId.Default with
                    {
                        ModelIndex = cubeId,
                        Color = color
                    });
                    _ecs.AddComponent(ref _platforms[i], ColisionBox.Default with
                    {
                        Box = colisionBox,
                    });
                    i++;
                }
            }
        }

        protected override void Render(double deltaTime)
        {
            
            _ecs.RenderSystems(deltaTime);
        }

        protected override void Update(double deltaTime)
        {
            if (_inputManager.IsKeyPressed(Silk.NET.Input.Key.Escape))
            {
                Close();
            }
            ref Transform playerTransform = ref _ecs.GetComponent<Transform>(in _cube);
            if (playerTransform.Position.Y < -10)
            {
                playerTransform.Position = new(0, 3, -40);

            }
            _ecs.UpdateSystems(deltaTime);
        }
    }
}
