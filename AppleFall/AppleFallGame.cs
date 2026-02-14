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

        public AppleFallGame(WindowOptions windowOptions) : base(windowOptions)
        {
            _trees = new Entity[20];
            _apples = new Entity[10];
            _basket = new Entity();
            _options = windowOptions;
        }

        protected override void Load()
        {
            IServiceCollection services = new ServiceCollection();
            InitializeDependencies(ref services);
            _provider = services.BuildServiceProvider();
            InitializeECS(ref _provider);
            IShaderManager shaderManager = _provider.GetRequiredService<IShaderManager>();

            IModelManager modelManager = _provider.GetRequiredService<IModelManager>();
            modelManager.LoadModel("apple2.obj");
            modelManager.LoadModel("tree.obj");
            modelManager.LoadModel("basket.obj");

            _ecs = _provider.GetRequiredService<IEntityComponentSystem>();
            Entity cam = _provider.GetRequiredService<Render>().GetCamera();
            ref Camera camera = ref _ecs.GetComponent<Camera>(in cam);
            ref Transform cameraPosition = ref _ecs.GetComponent<Transform>(in cam);
            camera.AspectRatio = _options.Size.X / (float)_options.Size.Y;
            cameraPosition.Position = new(0.0f,1.5f,-1.5f);
            int defaultShaderIndex = shaderManager.GetShaderIndex("default");
            _basket = _ecs.CreateEntity();
            _ecs.AddComponent(ref _basket, new Transform
            {
                Position = new(0.0f, 0.0f, -0.5f),
                Rotation = new(0.0f, Scalar.DegreesToRadians(90.0f), 0.0f),
                Scale = Vector3D<float>.One
            });
            _ecs.AddComponent(ref _basket, new ModelId
            {
                ModelIndex = modelManager.GetModelIndex("basket.obj"),
                ShaderIndex = defaultShaderIndex
            });
            _apples[0] = _ecs.CreateEntity();
            _ecs.AddComponent(ref _apples[0], new Transform
            {
                Position = new(0.0f, 0.0f, -0.5f),
                Rotation = Vector3D<float>.Zero,
                Scale = Vector3D<float>.One
            });
            _ecs.AddComponent(ref _apples[0], new ModelId
            {
                ModelIndex = modelManager.GetModelIndex("apple2.obj"),
                ShaderIndex = defaultShaderIndex
            });
            _trees[0] = _ecs.CreateEntity();
            _ecs.AddComponent(ref _trees[0], new Transform
            {
                Position = new(0.0f, 0.0f, 0.0f),
                Rotation = Vector3D<float>.Zero,
                Scale = Vector3D<float>.One
            });
            _ecs.AddComponent(ref _trees[0], new ModelId
            {
                ModelIndex = modelManager.GetModelIndex("tree.obj"),
                ShaderIndex = defaultShaderIndex
            });
        }

        protected override void Render(double deltaTime)
        {
            _ecs.UpdateSystems(deltaTime);
        }

        protected override void Update(double deltaTime)
        {
            
        }
    }
}
