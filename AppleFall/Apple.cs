using REVSharp.Core;

namespace AppleFall
{
    internal struct Apple : IComponent
    {
        public float Gravity { set; get; }
        public bool IsFalling { set; get; }
        public bool IsOnGround { set; get; }
    }
}
