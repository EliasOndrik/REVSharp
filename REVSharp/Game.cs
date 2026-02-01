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


namespace REVSharp
{
    public abstract class Game
    {
        
        private readonly IWindow _window;

        protected ECS Ecs { get; } = new ECS();
        protected GL _gl;
        protected Shader _shader;
        protected ModelManager _modelManager;
        
        public Game()
        {
            WindowOptions options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "REVSharp Game";
            
            _window = Window.Create(options);
            
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;

        }
        private void OnLoad()
        {
            // Initialize game resources here
            _gl = _window.CreateOpenGL();
            _modelManager = new ModelManager(_gl);
            _shader = new Shader(_gl, "Shaders/VertexShader.vers", "Shaders/FragmentShader.fras");
            _gl.ClearColor(Color.DarkCyan);
            
            _gl.Enable(EnableCap.DepthTest);
            if (Ecs == null)
                return;
            Ecs.RegisterComponent<Transform>();
            Ecs.RegisterComponent<ModelId>();
            Ecs.RegisterComponent<Vertex>();

            Render render = new(_shader, _modelManager);
            Ecs.RegisterSystem(render);
            uint mask = Ecs.GetComponentMask<Transform>() | Ecs.GetComponentMask<ModelId>();
            Ecs.SetSystemMask<Render>(mask);

            Load();
        }
        private void OnUpdate(double deltaTime)
        {
            // Update game logic here
            if (Ecs == null)
                return;
            
            Update(deltaTime);
        }
        private void OnRender(double deltaTime)
        {
            // Render game here
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _gl.Clear(ClearBufferMask.DepthBufferBit);
            _shader.Use();
            if (Ecs == null)
                return;
            Ecs.UpdateSystems((float)deltaTime);
            Render(deltaTime);
        }
        public void Run()
        {
            _window.Run();
        }
        protected abstract void Load();
        protected abstract void Update(double deltaTime);
        protected abstract void Render(double deltaTime);
        
    }
}
