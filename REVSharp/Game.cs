using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using REVSharp.Core;
using REVSharp.Components;
using System.Drawing;
using REVSharp.Behaviours;
using REVSharp.Entities;

namespace REVSharp
{
    internal class Game
    {
        private readonly IWindow _window;
        public static ECS? Ecs { get; private set; }
        private GL _gl;
        private Shader _shader;
        private Cube Cube;
        public Game()
        {
            WindowOptions options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "REVSharp Game";
            Ecs = new ECS();
            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
        }
        private void OnLoad()
        {
            // Initialize game resources here
            _gl = _window.CreateOpenGL();
            _gl.ClearColor(Color.AliceBlue);
            _shader = new Shader(_gl, "Shaders/VertexShader.vers", "Shaders/FragmentShader.fras");
            _gl.Enable(EnableCap.DepthTest);
            Ecs.RegisterComponent<Transform>();
            Ecs.RegisterComponent<Mesh>();
            
            Render render = new(_shader);
            Ecs.RegisterSystem(render);
            uint mask = Ecs.GetMask<Transform>() | Ecs.GetMask<Mesh>();
            Ecs.SetMask<Render>(mask);

            Cube = new(_gl);
        }
        private void OnUpdate(double deltaTime)
        {
            // Update game logic here
            Transform transform = Ecs.GetComponent<Transform>(Cube);
            Vector3D<float> rotation = new(Scalar.DegreesToRadians(50.0f)*(float)deltaTime, Scalar.DegreesToRadians(50.0f)*(float)deltaTime, 0f);
            transform.Rotation += rotation;
        }
        private void OnRender(double deltaTime)
        {
            // Render game here
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _gl.Clear(ClearBufferMask.DepthBufferBit);
            _shader.Use();
            Ecs.UpdateSystems((float)deltaTime);
        }
        public void Run()
        {
            _window.Run();
        }
    }
}
