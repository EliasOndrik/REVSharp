using Silk.NET.Maths;
using REVSharp.Core;

namespace REVSharp.Components
{
    public struct Transform : IComponent
    {
        public Vector3D<float> Position { set; get; }
        public Vector3D<float> Rotation { set; get; }
        public Vector3D<float> Scale { set; get; }
    }
}
