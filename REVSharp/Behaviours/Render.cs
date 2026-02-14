using REVSharp.Core;
using REVSharp.Components;  
using Silk.NET.Maths;
using REVSharp.ModelLoader;

namespace REVSharp.Behaviours
{
    public class Render : Behaviour
    {

        private readonly IModelManager _modelManager;
        private Entity _camera;

        private readonly IShaderManager _shaders;
        private int _currentShaderIndex;
        private IShader _currentShader;
        public Render(IShaderManager shaders, IModelManager modelManager)
        {
            _shaders = shaders;
            _modelManager = modelManager;
            _shaders.LoadShader("default", "Shaders/VertexShader.vers", "Shaders/FragmentShader.fras");
            _currentShaderIndex = _shaders.GetShaderIndex("default");
            _currentShader = _shaders.GetShader(_currentShaderIndex);
        }
        public override void Update(double deltaTime, ECS componenetManager)
        {
            uint cameraMask = componenetManager.GetComponentMask<Camera>() | componenetManager.GetComponentMask<Transform>();
            if ((_camera.ComponentMask & cameraMask) != cameraMask)
            {
                throw new Exception("Camera entity does not have proper components. It needs Transform and Camera.");
            }
            ref Camera cameraInfo = ref componenetManager.GetComponent<Camera>(in _camera);
            ref Transform cameraPosition = ref componenetManager.GetComponent<Transform>(in _camera);
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(cameraPosition.Position, cameraInfo.Target, cameraInfo.Up);
            Matrix4X4<float> projection = Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(cameraInfo.FieldOfView), cameraInfo.AspectRatio, cameraInfo.NearPlane, cameraInfo.FarPlane);

            foreach (var entity in Entities)
            {
                if (componenetManager == null)
                {
                    continue;
                }
                ref Transform transform = ref componenetManager.GetComponent<Transform>(in entity);
                ref ModelId mesh = ref componenetManager.GetComponent<ModelId>(in entity);
                if (mesh.ShaderIndex != _currentShaderIndex)
                {
                    _currentShaderIndex = mesh.ShaderIndex;
                    _currentShader = _shaders.GetShader(_currentShaderIndex);
                }
                _currentShader.Use();
                
                Matrix4X4<float> model = Matrix4X4.CreateScale(transform.Scale) *
                                        Matrix4X4.CreateRotationX(transform.Rotation.X) *
                                        Matrix4X4.CreateRotationY(transform.Rotation.Y) *
                                        Matrix4X4.CreateRotationZ(transform.Rotation.Z) *
                                        Matrix4X4.CreateTranslation(transform.Position);
                _currentShader.SetMatrix4x4("view", view);
                _currentShader.SetMatrix4x4("projection", projection);
                _currentShader.SetMatrix4x4("model", model);
                Matrix4X4.Invert(view * model, out Matrix4X4<float> result);
                _currentShader.SetMatrix4x4("inverseModel", Matrix4X4.Transpose(result));
                _currentShader.SetVector3D("objectColor", new(1f,1f,1f));
                _currentShader.SetVector3D("lightColor", Vector3D<float>.One);
                _currentShader.SetVector3D("lightPos", cameraPosition.Position);
                _modelManager.DrawModel(mesh.ModelIndex, _currentShader);

            }
        }
        public void SetCamera(Entity camera)
        {
            _camera = camera;
        }
        public Entity GetCamera()
        {
            return _camera;
        }
    }
}
