using REVSharp.Core;
using Silk.NET.Maths;
namespace REVSharp.Components
{
    public struct SimpleColisionBox : IComponent
    {
        public Vector2D<float> OffsetX { set; get; }
        public Vector2D<float> OffsetY { set; get; }
        public Vector2D<float> OffsetZ { set; get; }
    }
}
