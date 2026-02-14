using REVSharp.Core;
using Silk.NET.Maths;

namespace REVSharp.Components
{
    public struct ModelId : IComponent
    {
        public Vector3D<float> Color { set; get; }
        public int ModelIndex { set; get; }
        public int ShaderIndex { set; get; }
        public bool IsVisible { set; get; }
        public static ModelId Default => new ModelId
        {
            Color = Vector3D<float>.One,
            ModelIndex = -1,
            ShaderIndex = 0,
            IsVisible = true
        };
    }

}
