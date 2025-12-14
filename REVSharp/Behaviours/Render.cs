using REVSharp.Core;
using REVSharp.Components;  
using Silk.NET.Maths;

namespace REVSharp.Behaviours
{
    internal class Render : Behaviour
    {
        private readonly Shader _shader;
        public Render(Shader shader)
        {
            _shader = shader;

        }

        public override void Update(float deltaTime)
        {
            Vector3D<float> cameraPosition = new(0.0f, 0.0f, 5.0f);
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(cameraPosition, new(0.0f, 0.0f, 0.0f), new(0.0f, 1.0f, 0.0f));
            Matrix4X4<float> projection = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 4.0f, 800f / 600f, 0.1f, 100.0f);
            foreach (var entity in Entities)
            {
                Transform transform = CManager.GetComponent<Transform>(entity);
                Mesh mesh = CManager.GetComponent<Mesh>(entity);
                _shader.Use();
                Matrix4X4<float> model = Matrix4X4.CreateScale(transform.Scale) *
                                        Matrix4X4.CreateRotationX(transform.Rotation.X) *
                                        Matrix4X4.CreateRotationY(transform.Rotation.Y) *
                                        Matrix4X4.CreateRotationZ(transform.Rotation.Z) *
                                        Matrix4X4.CreateTranslation(transform.Position);
                _shader.SetMatrix4x4("view", view);
                _shader.SetMatrix4x4("projection", projection);
                _shader.SetMatrix4x4("model", model);
                mesh.Draw();

            }
        }
    }
}
