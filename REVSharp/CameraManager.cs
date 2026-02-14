using REVSharp.Core;
using REVSharp.Components;

namespace REVSharp
{
    public class CameraManager
    {
        private readonly IEntityComponentSystem _ecs;
        public CameraManager(IEntityComponentSystem ecs)
        {
            _ecs = ecs;
        }
        public Entity CreateThirdPersonCamera()
        {
            Entity camera = _ecs.CreateEntity();
            _ecs.AddComponent(ref camera, new Transform
            {
                Position = new(0.0f, 0.0f, 10.0f),
                Rotation = new(0.0f, 0.0f, 0.0f),
                Scale = new(1.0f, 1.0f, 1.0f)
                
            });
            _ecs.AddComponent(ref camera, new Camera
            {
                AspectRatio = 16.0f / 9.0f,
                FieldOfView = 45.0f,
                Target = new(0.0f, 0.0f, 0.0f),
                Up = new(0.0f, 1.0f, 0.0f),
                NearPlane = 0.1f,
                FarPlane = 100.0f,
                Distance = 10.0f
            });
            return camera;
        }
        
        public Entity CreateFirstPersonCamera()
        {
            Entity camera = _ecs.CreateEntity();
            _ecs.AddComponent(ref camera, new Transform
            {
                Position = new(0.0f, 0.0f, 0.0f),
                Rotation = new(0.0f, 0.0f, 0.0f),
                Scale = new(1.0f, 1.0f, 1.0f)

            });
            _ecs.AddComponent(ref camera, new Camera
            {
                AspectRatio = 16.0f / 9.0f,
                FieldOfView = 45.0f,
                Target = new(0.0f, 0.0f, -1.0f),
                Up = new(0.0f, 1.0f, 0.0f),
                NearPlane = 0.1f,
                FarPlane = 100.0f,
                Distance = 1.0f
            });
            return camera;
        }
        
    }
}
