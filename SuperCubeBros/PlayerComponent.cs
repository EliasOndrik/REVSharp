using REVSharp.Core;
using Silk.NET.Maths;
namespace SuperCubeBros
{
    internal struct PlayerComponent : IComponent
    {
        public Vector3D<float> Direction { set; get; }
    }
}
