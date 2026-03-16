using REVSharp.Core;
using Silk.NET.Maths;

namespace REVSharp.Components
{
    public struct ModelId : IComponent
    {
        public Matrix4X4<float> ModelMatrix { set; get; }
        public Matrix4X4<float> InverseModel { set; get; }
        public Vector3D<float> Color { set; get; }
        public int ModelIndex { set; get; }
        public int ShaderIndex { set; get; }
        public bool IsVisible { set; get; }
        public static ModelId Default => new()
        {
            ModelMatrix = Matrix4X4<float>.Identity,
            InverseModel = Matrix4X4<float>.Identity,
            Color = Vector3D<float>.One,
            ModelIndex = -1,
            ShaderIndex = 0,
            IsVisible = true
        };
    }

}
