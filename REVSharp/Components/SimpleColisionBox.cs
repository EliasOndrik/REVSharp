using REVSharp.Core;
namespace REVSharp.Components
{
    public struct SimpleColisionBox : IComponent
    {
        public float Right { set; get; }
        public float Left { set; get; }
        public float Top { set; get; }
        public float Bottom { set; get; }
        public float Front { set; get; }
        public float Back { set; get; }
        
    }
}
