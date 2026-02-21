using REVSharp;
using REVSharp.Core;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Microsoft.Extensions.DependencyInjection;
using REVSharp.ModelLoader;
using REVSharp.Components;
using REVSharp.Behaviours;


namespace AppleFall
{
    internal class AppleFallGame : Game
    {
        private Entity[] _trees;
        private Entity[] _apples;
        private Entity _basket;
        private IServiceProvider _provider;
        private WindowOptions _options;
        private IEntityComponentSystem _ecs;
        private Random _random;

        public AppleFallGame(WindowOptions windowOptions) : base(windowOptions)
        {
            _trees = new Entity[20];
            _apples = new Entity[10];
            _basket = new Entity();
            _options = windowOptions;
            _random = new Random();
        }

        protected override void Load()
        {
            IServiceCollection services = new ServiceCollection();
            InitializeDependencies(ref services);
            services.AddSingleton<AppleScript>();
            services.AddSingleton<BasketScript>();
            _provider = services.BuildServiceProvider();
            InitializeECS(ref _provider);
            IShaderManager shaderManager = _provider.GetRequiredService<IShaderManager>();

            IModelManager modelManager = _provider.GetRequiredService<IModelManager>();
            modelManager.LoadModel("apple2.obj");
            modelManager.LoadModel("tree.obj");
            modelManager.LoadModel("basket.obj");

            _ecs = _provider.GetRequiredService<IEntityComponentSystem>();

            _ecs.RegisterComponent<Apple>();
            AppleScript appleScripth = _provider.GetRequiredService<AppleScript>();
            _ecs.RegisterSystem(appleScripth);
            uint appleScriptMask = _ecs.GetComponentMask<Transform>() | _ecs.GetComponentMask<ModelId>() | _ecs.GetComponentMask<Apple>();
            _ecs.SetSystemMask<AppleScript>(appleScriptMask);

            _ecs.RegisterComponent<Basket>();
            BasketScript basketScript = _provider.GetRequiredService<BasketScript>();
            
            _ecs.RegisterSystem(basketScript);
            uint basketScriptMaskk = _ecs.GetComponentMask<Transform>() | _ecs.GetComponentMask<Basket>();
            _ecs.SetSystemMask<BasketScript>(basketScriptMaskk);


            Entity cam = _provider.GetRequiredService<Render>().GetCamera();
            basketScript.SetCamera(cam);
            ref Camera camera = ref _ecs.GetComponent<Camera>(in cam);
            ref Transform cameraPosition = ref _ecs.GetComponent<Transform>(in cam);
            camera.AspectRatio = _options.Size.X / (float)_options.Size.Y;
            cameraPosition.Position = new(0.0f,2f,-1.5f);
            int defaultShaderIndex = shaderManager.GetShaderIndex("default");

            _basket = _ecs.CreateEntity();
            _ecs.AddComponent(ref _basket, new Transform
            {
                Position = new(0.0f, 0.0f, -0.5f),
                Rotation = new(0.0f, Scalar.DegreesToRadians(90.0f), 0.0f),
                Scale = Vector3D<float>.One
            });
            _ecs.AddComponent(ref _basket, ModelId.Default with
            {
                ModelIndex = modelManager.GetModelIndex("basket.obj"),
                ShaderIndex = defaultShaderIndex
            });
            _ecs.AddComponent(ref _basket, new Basket());
            appleScripth.SetBasket(_basket);

            for (int i = 0; i < _apples.Length; i++)
            {
                _apples[0] = _ecs.CreateEntity();
                _ecs.AddComponent(ref _apples[i], Transform.Default with
                {
                    Position = new(0.0f, 1.0f, -0.5f)
                });
                _ecs.AddComponent(ref _apples[i], ModelId.Default with
                {
                    ModelIndex = modelManager.GetModelIndex("apple2.obj"),
                    ShaderIndex = defaultShaderIndex
                });
                _ecs.AddComponent(ref _apples[i], new Apple
                {
                    Gravity = 0.0f,
                    Timer = i*0.5f,
                    State = AppleState.Waiting
                });
            }
            Vector3D<float> treePositions = Vector3D<float>.Zero;
            for (int i = 0; i < _trees.Length; i++)
            {
                _trees[i] = _ecs.CreateEntity();
                _ecs.AddComponent(ref _trees[i], new Transform
                {
                    Position = treePositions,
                    Rotation = Vector3D<float>.Zero,
                    Scale = Vector3D<float>.One
                });
                treePositions = new(_random.Next(-10,10),0f, _random.Next(-10, 10));
                _ecs.AddComponent(ref _trees[i], ModelId.Default with
                {
                    ModelIndex = modelManager.GetModelIndex("tree.obj"),
                    ShaderIndex = defaultShaderIndex
                });
            }
            
        }

        protected override void Render(double deltaTime)
        {
            _ecs.RenderSystems(deltaTime);
        }

        protected override void Update(double deltaTime)
        {
            _ecs.UpdateSystems(deltaTime);
        }
    }
}
