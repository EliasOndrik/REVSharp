using REVSharp;
using REVSharp.Components;
using REVSharp.Core;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Microsoft.Extensions.DependencyInjection;
using REVSharp.ModelLoader;
using REVSharp.Behaviours;

namespace ExampleGame
{
    internal class Game1 : Game
    {
        //private Entity Entity;
        private Entity en;
        private IEntityComponentSystem Ecs;
        private IServiceProvider serviceProvider;
        private IModelManager _modelManager;
        private Entity camera;
        private WindowOptions windowOptions;

        public Game1(WindowOptions options) : base(options)
        {
            windowOptions = options;
        }

        protected override void Load()
        {
            IServiceCollection services = new ServiceCollection();
            InitializeDependencies(ref services);
            serviceProvider = services.BuildServiceProvider();
            _modelManager = serviceProvider.GetRequiredService<IModelManager>();
            InitializeECS(ref serviceProvider);
            Ecs = serviceProvider.GetRequiredService<IEntityComponentSystem>();
            Render render = serviceProvider.GetRequiredService<Render>();
            camera = render.GetCamera();
            ref Transform cameraTransform = ref Ecs.GetComponent<Transform>(in camera);
            cameraTransform.Position = new Vector3D<float>(0.0f, 0.0f, -10.0f);
            ref Camera camearInfo = ref Ecs.GetComponent<Camera>(in camera);
            camearInfo.AspectRatio = windowOptions.Size.X / (float)windowOptions.Size.Y;

            Ecs.RegisterSystem(new RotateBanana());
            uint mask = Ecs.GetComponentMask<Transform>();
            Ecs.SetSystemMask<RotateBanana>(mask);

            //Entity = Ecs.CreateEntity();
            //Ecs.AddComponent(Entity, new Transform());
            //Ecs.AddComponent(Entity, new Model(_gl, "cube.obj"));
            //Ecs.AddComponent(Entity, new Vertex());
            _modelManager.LoadModel("vaza.dae");
            int modelId = _modelManager.GetModelIndex("vaza.dae");
            IShaderManager shaders = serviceProvider.GetRequiredService<IShaderManager>();
            int shaderId = shaders.GetShaderIndex("default");
            for (int i = 0; i < 1000; i++)
            {
                en = Ecs.CreateEntity();
                Ecs.AddComponent(ref en, new Transform
                {
                    Position = new Vector3D<float>(0.0f, -2.0f, 0.0f+i),
                    Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
                    Scale = new Vector3D<float>(1f, 1f, 1f)
                });
                Ecs.AddComponent(ref en, new ModelId { 
                    ModelIndex = modelId,
                    ShaderIndex = shaderId
                });
            }
            
            //Ecs.AddComponent(en, new Vertex());
        }

        protected override void Render(double deltaTime)
        {
            Ecs.UpdateSystems(deltaTime);
        }

        protected override void Update(double deltaTime)
        {
            
        }
    }
}
