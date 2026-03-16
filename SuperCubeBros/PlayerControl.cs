using REVSharp.Core;
using REVSharp.Components;
using REVSharp.Input;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Assimp;

namespace SuperCubeBros
{
    internal class PlayerControl : Behaviour
    {
        private IInputManager _inputManager;
        private Entity _camera;
        private readonly float _moveSpeed = 15f;
        private readonly float _gravity = 50f;
        private readonly float _jumpStrength = 24f;
        public PlayerControl(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }
        public override void OnRender(double deltaTime, ECS componentManager)
        {
            
        }

        public override void OnUpdate(double deltaTime, ECS componentManager)
        {
            if (Entities.Count == 0)
            {
                return;
            }
            Entity player = Entities[0];
            ref Transform playerTransform = ref componentManager.GetComponent<Transform>(in player);
            ref ColisionBox playerColisionBox = ref componentManager.GetComponent<ColisionBox>(in player);
            ref PlayerComponent playerComponent = ref componentManager.GetComponent<PlayerComponent>(in player);

            ref Transform cameraTransform = ref componentManager.GetComponent<Transform>(in _camera);
            Vector3D<float> cameraDirection = playerTransform.Position - cameraTransform.Position;
            cameraDirection.Y = 0;
            cameraDirection = Vector3D.Normalize(cameraDirection);
            Vector3D<float> rightVector = Vector3D.Cross(Vector3D<float>.UnitY, cameraDirection);
            Vector3D<float> nextPosition = Vector3D<float>.Zero;
            nextPosition.Y = playerComponent.Direction.Y - _gravity * (float)deltaTime;
            if (_inputManager.IsKeyPressed(Key.W))
            {
                nextPosition += cameraDirection * _moveSpeed;
            }
            if (_inputManager.IsKeyPressed(Key.S))
            {
                nextPosition -= cameraDirection * _moveSpeed;
            }
            if (_inputManager.IsKeyPressed(Key.A))
            {
                nextPosition += rightVector * _moveSpeed;
            }
            if (_inputManager.IsKeyPressed(Key.D))
            {
                nextPosition -= rightVector * _moveSpeed;
            }

            if (((playerColisionBox.Direction & ColisionDirection.Left) == ColisionDirection.Left && nextPosition.X < 0) || ((playerColisionBox.Direction & ColisionDirection.Right) == ColisionDirection.Right && nextPosition.X > 0))
            {
                nextPosition.X = 0;
            }

            if (((playerColisionBox.Direction & ColisionDirection.Front) == ColisionDirection.Front && nextPosition.Z > 0 )|| ((playerColisionBox.Direction & ColisionDirection.Back) == ColisionDirection.Back && nextPosition.Z < 0))
            {
                nextPosition.Z = 0;
            }

            if ((playerColisionBox.Direction & ColisionDirection.Bottom) == ColisionDirection.Bottom)
            {
                nextPosition.Y = 0;
                if (_inputManager.IsKeyPressed(Key.Space))
                {
                    nextPosition.Y = _jumpStrength;
                    
                }
            }
            //Console.WriteLine(deltaTime);
            playerComponent.Direction = nextPosition;
            playerTransform.Position += nextPosition * (float)deltaTime;
        }
        public void SetCamera(Entity camera)
        {
            _camera = camera;
        }
    }
}
