
using Silk.NET.Input;
using REVSharp.Components;
using REVSharp.Core;
using REVSharp.Input;
using Silk.NET.Maths;

namespace AppleFall
{
    internal class BasketScript : Behaviour
    {
        private IInputManager _input;
        private Entity _camera;
        private float _rotation;
        private const float rotationSpeed = 1f;
        private const float cameraRadius = 1.5f;
        private const float basketRadius = 0.5f;
        public BasketScript(IInputManager inputManager)
        {
            _input = inputManager;
            _rotation = 0f;
        }
        
        public override void OnRender(double deltaTime, ECS componentManager)
        {
            
        }

        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            foreach (var entity in Entities)
            {
                ref Transform basketTransform = ref componentManager.GetComponent<Transform>(in entity);
                if (_input.IsKeyPressed(Key.Left) || _input.IsKeyPressed(Key.A))
                {
                    _rotation -= rotationSpeed * (float)deltaTime;
                }
                if (_input.IsKeyPressed(Key.Right) || _input.IsKeyPressed(Key.D))
                {
                    _rotation += rotationSpeed * (float)deltaTime;
                }
                
                basketTransform.Position = new(Scalar.Sin(_rotation) * basketRadius, basketTransform.Position.Y, Scalar.Cos(_rotation) * basketRadius);
                basketTransform.Rotation = new(basketTransform.Rotation.X, _rotation + Scalar.DegreesToRadians(90f), basketTransform.Rotation.Z);
            }
            float distance = _input.GetMouseScroll();
            
            ref Transform cameraTransform = ref componentManager.GetComponent<Transform>(in _camera);
            cameraTransform.Position = new(Scalar.Sin(_rotation) * cameraRadius, cameraTransform.Position.Y + distance, Scalar.Cos(_rotation) * cameraRadius);
        }
        public void SetCamera(Entity camera)
        {
            _camera = camera;
        }
    }
}
