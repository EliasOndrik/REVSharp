using REVSharp.Components;
using REVSharp.Core;

namespace SuperCubeBros
{
    internal struct ColisionBox : IComponent
    {
        public SimpleColisionBox Box { set; get; }
        public ColisionType Type { set; get; }
        public ColisionDirection Direction { set; get; }
        public static ColisionBox Default => new ColisionBox
        {
            Box = new SimpleColisionBox(),
            Type = ColisionType.Static,
            Direction = ColisionDirection.None
        };

    }
    public enum ColisionType
    {
        Static,
        Dynamic
    }
    [Flags]
    public enum ColisionDirection : byte
    {
        None = 0,
        Top = 1 << 0,
        Bottom = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
        Front = 1 << 4,
        Back = 1 << 5,
        Side = Left | Right | Front | Back
    }
}
