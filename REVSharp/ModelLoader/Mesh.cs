using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace REVSharp.ModelLoader
{
    public class Mesh
    {
        public float[] Vertices{ get; set; }
        public uint[] Indices { get; set; }
        public List<Texture> Textures { get; set; }
        public Vector3D<float> Color { get; set; } = new Vector3D<float>(1.0f, 1.0f, 1.0f);
        private readonly GL _gl;
        private uint _vao;
        private uint _vbo;
        private uint _ebo;
        
        public Mesh(GL gl,float[] vertices, uint[] indices, List<Texture> textures)
        {
            _gl = gl;
            Vertices = vertices;
            Indices = indices;
            Textures = textures;
            SetupMesh();
        }
        private unsafe void SetupMesh()
        {
            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            fixed(float* buffer = Vertices)_gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(Vertices.Length * sizeof(float)), buffer, BufferUsageARB.StaticDraw);
            
            if (Indices.Length != 0)
            {
                _ebo = _gl.GenBuffer();
                _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
                fixed (uint* buffer = Indices) _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(Indices.Length * sizeof(uint)), buffer, BufferUsageARB.StaticDraw);
            }

            uint positionLocation = 0;
            _gl.EnableVertexAttribArray(positionLocation);
            _gl.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)0);

            uint normalLocation = 1;
            _gl.EnableVertexAttribArray(normalLocation);
            _gl.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)(3 * sizeof(float)));

            uint texCoordLocation = 2;
            _gl.EnableVertexAttribArray(texCoordLocation);
            _gl.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*)(6 * sizeof(float)));

            _gl.BindVertexArray(0);
        }
        public unsafe void Draw()
        {
            foreach (var text in Textures)
            {
                _gl.BindTexture(TextureTarget.Texture2D, text.Id);
                _gl.ActiveTexture(GLEnum.Texture0);
            }
            _gl.BindVertexArray(_vao);
            if (Indices.Length != 0)
            {
                _gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
            }
            else
            {
                _gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)(Vertices.Length / 8));
            }
            _gl.BindVertexArray(0);
        }
    }
}
