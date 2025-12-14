using REVSharp.Core;
using REVSharp.Components;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
namespace REVSharp.Entities
{
    internal class Cube : Entity
    {
        private GL _gl;
        public Cube(GL gl)
        {
            _gl = gl;
            Transform transform = new()
            {
                Position = new Vector3D<float>(0.0f, 0.0f, 0.0f),
                Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
                Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
            };
            Game.Ecs?.AddComponent(this, transform);
            float[] verteces = [
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f, 0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f
            ];
            Mesh mesh = new(_gl, verteces);
            Game.Ecs?.AddComponent(this, mesh);
        }
    }
}
