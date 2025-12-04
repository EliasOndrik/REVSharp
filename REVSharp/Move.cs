using REVSharp.Core;
using Silk.NET.Maths;

namespace REVSharp
{

    internal class Move : Behaviour
    {
        public override void Update(float deltaTime)
        {
            foreach (var entity in Entities)
            {
                if (CManager == null)
                {
                    continue;
                }
                Transform t =  CManager.GetComponent<Transform>(entity);
                Vector3D<float> move = new(1.0f, 1.0f, 0.0f);
                t.Position += move;
            }
        }
    }
}
