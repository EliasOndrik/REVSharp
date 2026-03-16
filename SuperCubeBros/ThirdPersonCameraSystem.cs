using REVSharp.Components;
using REVSharp.Core;
using REVSharp.Input;
using Silk.NET.Maths;

namespace SuperCubeBros
{

    internal class ThirdPersonCameraSystem : Behaviour
    {
        private IInputManager _inputManager;
        private Entity _target;
        private int _screenWidth;
        private int _screenHeight;
        
        public ThirdPersonCameraSystem(IInputManager inputManager)
        {
            _inputManager = inputManager;
            _screenHeight = 0;
            _screenWidth = 0;
        }
        public override void OnRender(double deltaTime, ECS componentManager)
        {
            
        }

        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            Vector2D<float> mousePosition = new(_screenWidth / 2, _screenHeight / 2);
            mousePosition = (_inputManager.GetMousePosition() - mousePosition) * (float)deltaTime;
            Entity camera = Entities[0];
            ref Transform cameraTransform = ref componentManager.GetComponent<Transform>(ref camera);
            float corectionY = mousePosition.Y;
            if (cameraTransform.Rotation.Y + corectionY >= Scalar.DegreesToRadians( 89f))
            {
                corectionY = Scalar.DegreesToRadians(89f) - cameraTransform.Rotation.Y;
            }
            if (cameraTransform.Rotation.Y + corectionY <= -Scalar.DegreesToRadians(89f))
            {
                corectionY = -Scalar.DegreesToRadians(89f) - cameraTransform.Rotation.Y;
            }
            cameraTransform.Rotation += new Vector3D<float>(-mousePosition.X, corectionY, 0);
            
            ref Camera cameraComponent = ref componentManager.GetComponent<Camera>(ref camera);
            ref Transform targetTransform = ref componentManager.GetComponent<Transform>(ref _target);
            cameraTransform.Position = targetTransform.Position + new Vector3D<float>(cameraComponent.Distance * Scalar.Cos(cameraTransform.Rotation.Y) * Scalar.Sin(cameraTransform.Rotation.X), cameraComponent.Distance * Scalar.Sin(cameraTransform.Rotation.Y), cameraComponent.Distance * Scalar.Cos(cameraTransform.Rotation.X) * Scalar.Cos(cameraTransform.Rotation.Y));
            cameraComponent.Target = targetTransform.Position;
            _inputManager.SetMousePosition(new(_screenWidth / 2, _screenHeight / 2));
            
            
        }
        public void SetTarget(Entity target)
        {
            _target = target;
        }
        public void SetScreenSize(int widht, int height)
        {
            _screenWidth = widht;
            _screenHeight = height;
        }
    }
}
