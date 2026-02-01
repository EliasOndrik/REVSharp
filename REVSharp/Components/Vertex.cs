using Silk.NET.Maths;
using REVSharp.Core;


namespace REVSharp.Components
{
    public struct Vertex : IComponent
    {
        public Vector3D<float> Position { get; set; }
        public Vector3D<float> Normal { get; set; }
        public Vector2D<float> TexCoords { get; set; }
    }
}
