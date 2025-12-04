using Silk.NET.Maths;
using REVSharp.Core;

namespace REVSharp
{
    internal class Transform : IComponent
    {
        public Vector3D<float> Position { get; set; }
        public Vector3D<float> Rotation { get; set; }
        public Vector3D<float> Scale { get; set; }
    }
}
