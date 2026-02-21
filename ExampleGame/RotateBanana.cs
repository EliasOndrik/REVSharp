using REVSharp.Components;
using REVSharp.Core;
using Silk.NET.Maths;

namespace ExampleGame
{
    internal class RotateBanana : Behaviour
    {
        public override void OnRender(double deltaTime, ECS componentManager)
        {
            
        }

        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            foreach (var item in Entities)
            {
                ref Transform transform = ref componentManager.GetComponent<Transform>(in item);
                Vector3D<float> rotation = new(Scalar.DegreesToRadians(50.0f) * (float)deltaTime, Scalar.DegreesToRadians(50.0f) * (float)deltaTime, 0f);
                transform.Rotation += rotation;
                
            }
        }
    }
}
