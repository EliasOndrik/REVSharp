using Silk.NET.Maths;
using Silk.NET.OpenGL;
using REVSharp.Core;
using System.Runtime.CompilerServices;

namespace REVSharp.Components
{
    internal class Mesh : IComponent
    {
        public float[] Vertices { get; set; }
        //public uint[] Indices { get; set; }
        //public List<Texture> Textures { get; set; }
        private readonly GL _gl;
        private uint _vao;
        private uint _vbo;
        //private uint _ebo;
        public Mesh(GL gl,float[] vertices)
        {
            _gl = gl;
            Vertices = vertices;
            //Indices = indices;
            SetupMesh();
        }
        private unsafe void SetupMesh()
        {
            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            fixed(float* buffer = Vertices)_gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(Vertices.Length * sizeof(float)), buffer, BufferUsageARB.StaticDraw);

            uint positionLocation = 0;
            _gl.EnableVertexAttribArray(positionLocation);
            _gl.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)0);

            uint colorLocation = 1;
            _gl.EnableVertexAttribArray(colorLocation);
            _gl.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)(3 * sizeof(float)));

        }
        public void Draw()
        {
            _gl.BindVertexArray(_vao);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)(Vertices.Length / 6));
            _gl.BindVertexArray(0);
        }
    }
}
