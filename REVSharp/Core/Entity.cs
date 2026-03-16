using System.Numerics;

namespace REVSharp.Core
{
    public struct Entity
    {
        public uint Id { get; internal set; }
        public uint ComponentMask { get; internal set; }
    }
}
