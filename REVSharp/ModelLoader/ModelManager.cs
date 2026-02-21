using Silk.NET.OpenGL;
using Silk.NET.Assimp;
using REVSharp.Components;
using Silk.NET.Maths;

namespace REVSharp.ModelLoader
{
    public interface IModelManager
    {
        public void LoadModel(string path);

        public int GetModelIndex(string path);

        public void DrawModel(int index, IShader shader);
        public bool IsColiding(int index1, Vector3D<float> position1, int index2, Vector3D<float> position2);
    }
    public class ModelManager : IModelManager
    {
        private readonly List<Model> _loadedModels;
        private readonly Dictionary<string, int> _modelIndex;
        private readonly List<SimpleColisionBox> _colisionBoxes;
        private readonly GL _gl;
        
        public ModelManager(GL openGl)
        {
            _modelIndex = [];
            _loadedModels = [];
            _gl = openGl;
            _colisionBoxes = [];
        }
        public void LoadModel(string path)
        {
            if(_modelIndex.ContainsKey(path))
            {
                return;
            }
            _loadedModels.Add(new Model(_gl, path));
            _modelIndex.Add(path, _loadedModels.Count - 1);
            GenerateSimpleColisionBox(_loadedModels.Count - 1);
        }

        public int GetModelIndex(string path)
        {
            if (_modelIndex.TryGetValue(path, out int value))
            {
                return value;
            }
            return -1;
        }

        public void DrawModel(int index, IShader shader)
        {
            if (index < 0 || index >= _loadedModels.Count)
            {
                return;
            }
            _loadedModels[index].Draw(shader);
        }
        private void GenerateSimpleColisionBox(int index)
        {
            SimpleColisionBox colisionBox = new();
            foreach (var mesh in _loadedModels[index].Meshes)
            {
                for (int i = 0; i < mesh.Vertices.Length / 8; i++)
                {
                    colisionBox.OffsetX = new Vector2D<float>(Math.Min(colisionBox.OffsetX.X, mesh.Vertices[i*8]), Math.Max(colisionBox.OffsetX.Y, mesh.Vertices[i * 8]));
                    colisionBox.OffsetY = new Vector2D<float>(Math.Min(colisionBox.OffsetY.X, mesh.Vertices[i * 8 + 1]), Math.Max(colisionBox.OffsetY.Y, mesh.Vertices[i * 8 + 1]));
                    colisionBox.OffsetZ = new Vector2D<float>(Math.Min(colisionBox.OffsetZ.X, mesh.Vertices[i * 8 + 2]), Math.Max(colisionBox.OffsetZ.Y, mesh.Vertices[i * 8 + 2]));
                }
            }
            _colisionBoxes.Add(colisionBox);
        }

        public bool IsColiding(int index1, Vector3D<float> position1, int index2, Vector3D<float> position2)
        {
            SimpleColisionBox box1 = _colisionBoxes[index1];
            SimpleColisionBox box2 = _colisionBoxes[index2];

            return position1.X + box1.OffsetX.X < position2.X + box2.OffsetX.Y &&
                position1.X + box1.OffsetX.Y > position2.X + box2.OffsetX.X &&
                position1.Y + box1.OffsetX.X < position2.Y + box2.OffsetX.Y &&
                position1.Y + box1.OffsetX.Y > position2.Y + box2.OffsetX.X &&
                position1.Z + box1.OffsetX.X < position2.Z + box2.OffsetX.Y &&
                position1.Z + box1.OffsetX.Y > position2.Z + box2.OffsetX.X;
        }
    }
}
