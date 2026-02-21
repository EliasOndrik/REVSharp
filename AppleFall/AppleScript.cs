using REVSharp.Core;
using REVSharp.Components;
using System.Drawing;
using Silk.NET.Maths;
using REVSharp.ModelLoader;

namespace AppleFall
{
    internal class AppleScript : Behaviour
    {
        private const float Gravity = 0.001f;
        private const float Radius = 0.5f;
        private Random _random;
        private Entity _basket;
        private IModelManager _modelManager;
        public AppleScript(IModelManager modelManager)
        {
            _random = new();
            _modelManager = modelManager;
        }
        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            foreach (var entity in Entities)
            {
                ref Apple apple = ref componentManager.GetComponent<Apple>(in entity);
                ref Transform transform = ref componentManager.GetComponent<Transform>(in entity);
                ref ModelId modelId = ref componentManager.GetComponent<ModelId>(in entity);
                apple.Timer -= (float)deltaTime;
                switch (apple.State)
                {
                    case AppleState.Waiting:
                        
                        if (apple.Timer < 0)
                        {
                            modelId.Color = Vector3D<float>.One;
                            apple.Gravity = 0.0f;
                            float angle = (float)_random.NextDouble() * 360;
                            transform.Position = new(Scalar.Sin(angle) * Radius, 1.0f, Scalar.Cos(angle) * Radius);
                            transform.Rotation = new(_random.Next(360), _random.Next(360), _random.Next(360));
                            modelId.IsVisible = true;
                            apple.State = AppleState.Falling;
                        }
                        break;

                    case AppleState.Falling:

                        apple.Gravity += Gravity * (float)deltaTime;
                        transform.Position = new(transform.Position.X, transform.Position.Y - apple.Gravity, transform.Position.Z);
                        transform.Rotation = new(transform.Rotation.X + (float)deltaTime, transform.Rotation.Y + (float)deltaTime, transform.Rotation.Z + (float)deltaTime);
                        ref Transform basketPosition = ref componentManager.GetComponent<Transform>(in _basket);
                        ref ModelId basketModel = ref componentManager.GetComponent<ModelId>(in _basket);
                        
                        if (transform.Position.Y < -0.1f)
                        {
                            apple.Timer = 2.0f;
                            modelId.Color = new(80.0f/255.0f, 100.0f/255.0f, 69.0f/255.0f);
                            apple.State = AppleState.OnGround;
                        }
                        if (transform.Position.Y < -0.1f && _modelManager.IsColiding(basketModel.ModelIndex, basketPosition.Position, modelId.ModelIndex, transform.Position))
                        {
                            apple.Timer = 2.0f;
                            modelId.Color = new(Color.Gold.R, Color.Gold.G, Color.Gold.B);
                            apple.State = AppleState.Caught;

                        }
                        break;

                    case AppleState.OnGround:

                        if (apple.Timer < 0)
                        {
                            apple.State = AppleState.Waiting;
                            apple.Timer = (float)_random.NextDouble();
                            modelId.IsVisible = false;
                        }
                        break;

                    case AppleState.Caught:

                        if (apple.Timer < 0)
                        {
                            apple.State = AppleState.Waiting;
                            apple.Timer = (float)_random.NextDouble();
                            modelId.IsVisible = false;
                        }
                        break;
                }
                
            }
        }
        public void SetBasket(Entity basket)
        {
            _basket = basket;
        }

        public override void OnRender(double deltaTime, ECS componentManager)
        {
           
        }
    }
}
