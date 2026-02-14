using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using REVSharp.Core;
using REVSharp.Components;
using REVSharp.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using REVSharp.ModelLoader;
using REVSharp.Input;


namespace REVSharp
{
    public abstract class Game
    {
        
        private readonly IWindow _window;

        private GL _gl;
        
        public Game(WindowOptions windowOptions)
        {
            _window = Window.Create(windowOptions);
            
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;

        }
        private void OnLoad()
        {
            // Initialize game resources here
            _gl = _window.CreateOpenGL();
            
            
            _gl.ClearColor(Color.DarkCyan);
            
            _gl.Enable(EnableCap.DepthTest);
            

            Load();
        }
        private void OnUpdate(double deltaTime)
        {
            // Update game logic here
            
            
            Update(deltaTime);
        }
        private void OnRender(double deltaTime)
        {
            // Render game here
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _gl.Clear(ClearBufferMask.DepthBufferBit);
            
            Render(deltaTime);
        }
        public void Run()
        {
            _window.Run();
        }
        protected abstract void Load();
        protected abstract void Update(double deltaTime);
        protected abstract void Render(double deltaTime);
        public void InitializeDependencies(ref IServiceCollection services)
        {
            services.AddSingleton<IEntityComponentSystem,ECS>();
            services.AddSingleton<IShaderManager,ShaderManager>();
            services.AddSingleton<IModelManager,ModelManager>();
            services.AddSingleton<Render>();
            services.AddSingleton(_window.CreateInput());
            services.AddSingleton(_gl);
            services.AddSingleton<IInputManager,InputManager>();
            services.AddSingleton<CameraManager>();
        }
        public void InitializeECS(ref IServiceProvider provider)
        {
            IEntityComponentSystem ecs = provider.GetRequiredService<IEntityComponentSystem>();
            ecs.RegisterComponent<Transform>();
            ecs.RegisterComponent<ModelId>();
            ecs.RegisterComponent<Camera>();

            uint renderMask = ecs.GetComponentMask<Transform>() | ecs.GetComponentMask<ModelId>();
            Render renderSystem = provider.GetRequiredService<Render>();
            ecs.RegisterSystem(renderSystem);
            ecs.SetSystemMask<Render>(renderMask);
            CameraManager cameraManager = provider.GetRequiredService<CameraManager>();
            Entity camera = cameraManager.CreateThirdPersonCamera();
            renderSystem.SetCamera(camera);

        }
    }
}
