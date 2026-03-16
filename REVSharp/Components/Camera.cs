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

        public static Camera FirstPersonCamera => new()
        {
            AspectRatio = 16.0f / 9.0f,
            FieldOfView = 45.0f,
            Target = new(0.0f, 0.0f, -1.0f),
            Up = new(0.0f, 1.0f, 0.0f),
            NearPlane = 0.1f,
            FarPlane = 100.0f,
            Distance = 1.0f
        };
        public static Camera ThirdPersonCamera => new()
        {
            AspectRatio = 16.0f / 9.0f,
            FieldOfView = 45.0f,
            Target = new(0.0f, 0.0f, 0.0f),
            Up = new(0.0f, 1.0f, 0.0f),
            NearPlane = 0.1f,
            FarPlane = 100.0f,
            Distance = 10.0f
        };
    }
}
