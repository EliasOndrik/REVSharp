using REVSharp.Core;
using REVSharp.Components;

namespace AppleFall
{
    internal class AppleScript : Behaviour
    {
        private const float Gravity = 9.81f;
        public override void Update(double deltaTime, ECS componentManager)
        {
            foreach (var entity in Entities)
            {
                ref Apple apple = ref componentManager.GetComponent<Apple>(in entity);
                ref Transform transform = ref componentManager.GetComponent<Transform>(in entity);

                apple.Gravity += Gravity * (float)deltaTime;
                transform.Position = new(transform.Position.X, transform.Position.Y - apple.Gravity, transform.Position.Z);

                if (transform.Position.Y < 0.0f)
                {
                    apple.IsFalling = false;
                    apple.IsOnGround = true;
                }

            }
        }
    }
}
