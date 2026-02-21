using REVSharp.Core;

namespace AppleFall
{
    internal struct Apple : IComponent
    {
        public float Gravity { set; get; }
        public float Timer { set; get; }
        public AppleState State { set; get; }
    }
    internal enum AppleState
    {
        Waiting,
        Falling,
        OnGround,
        Caught
    }
}
