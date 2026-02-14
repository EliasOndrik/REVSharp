using REVSharp.Core;
using Silk.NET.Maths;
namespace REVSharp.Components
{
    public struct Camera : IComponent
    {
        public Vector3D<float> Target { set; get; }
        public Vector3D<float> Up { set; get; }
        public float FieldOfView { set; get; }
        public float AspectRatio { set; get; }
        public float NearPlane { set; get; }
        public float FarPlane { set; get; }
        public float Distance { set; get; }
    }
}
