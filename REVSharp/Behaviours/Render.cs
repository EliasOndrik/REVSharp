using REVSharp.Core;
using REVSharp.Components;  
using Silk.NET.Maths;
using REVSharp.ModelLoader;

namespace REVSharp.Behaviours
{
    internal class Render(Shader shader, ModelManager modelManager) : Behaviour
    {
        private readonly Shader _shader = shader;
        private readonly ModelManager _modelManager = modelManager;

        public override void Update(double deltaTime, ECS componenetManager)
        {
            Vector3D<float> cameraPosition = new(0.0f, 0.0f, 10.0f);
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(cameraPosition, new(0.0f, 0.0f, 0.0f), new(0.0f, 1.0f, 0.0f));
            Matrix4X4<float> projection = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 4.0f, 800f / 600f, 0.1f, 100.0f);

            foreach (var entity in Entities)
            {
                if (componenetManager == null)
                {
                    continue;
                }
                ref Transform transform = ref componenetManager.GetComponent<Transform>(in entity);
                ref ModelId mesh = ref componenetManager.GetComponent<ModelId>(in entity);
                _shader.Use();
                
                Matrix4X4<float> model = Matrix4X4.CreateScale(transform.Scale) *
                                        Matrix4X4.CreateRotationX(transform.Rotation.X) *
                                        Matrix4X4.CreateRotationY(transform.Rotation.Y) *
                                        Matrix4X4.CreateRotationZ(transform.Rotation.Z) *
                                        Matrix4X4.CreateTranslation(transform.Position);
                _shader.SetMatrix4x4("view", view);
                _shader.SetMatrix4x4("projection", projection);
                _shader.SetMatrix4x4("model", model);
                Matrix4X4.Invert(model, out Matrix4X4<float> result);
                _shader.SetMatrix4x4("inverseModel", Matrix4X4.Transpose(result));
                _shader.SetVector3D("objectColor", new(0.5f,0.6f,0.2f));
                _shader.SetVector3D("lightColor", new Vector3D<float>(1.0f, 1.0f, 1.0f));
                _shader.SetVector3D("lightPos", new Vector3D<float>(1.0f,1.0f,1.0f));
                _modelManager.DrawModel(mesh.ModelIndex);

            }
        }
    }
}
