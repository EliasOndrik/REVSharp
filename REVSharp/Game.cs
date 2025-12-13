using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using REVSharp.Core;
using System.Drawing;

namespace REVSharp
{
    internal class Game
    {
        private readonly IWindow _window;
        public static ECS? Ecs { get; private set; }
        private GL _gl;
        private Shader _shader;
        public Game()
        {
            WindowOptions options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "REVSharp Game";
            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            Ecs = new ECS();
        }
        private void OnLoad()
        {
            // Initialize game resources here
            _gl = _window.CreateOpenGL();
            _gl.ClearColor(Color.AliceBlue);
            _shader = new Shader(_gl, "Shaders/VertexShader.vers", "Shaders/FragmentShader.fras");

        }
        private void OnUpdate(double deltaTime)
        {
            // Update game logic here
            
        }
        private void OnRender(double deltaTime)
        {
            // Render game here
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
        }
        public void Run()
        {
            _window.Run();
        }
    }
}
