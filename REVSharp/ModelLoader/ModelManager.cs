using Silk.NET.OpenGL;
using REVSharp.Components;
using Silk.NET.Maths;

namespace REVSharp.ModelLoader
{
    public interface IModelManager
    {
        public void LoadModel(string path);

        public int GetModelIndex(string path);

        public void DrawModel(int index, IShader shader);
        public void CreateColisionBox(string path, out Vector3D<float> leftBottomBack, out Vector3D<float> rightTopFront);
        public bool IsColiding(int index1, Vector3D<float> position1, int index2, Vector3D<float> position2);
        public bool DeleteModel(string path);
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
                    colisionBox.Left = Math.Min(colisionBox.Left, mesh.Vertices[i * 8]);
                    colisionBox.Right = Math.Max(colisionBox.Right, mesh.Vertices[i * 8]);
                    colisionBox.Bottom = Math.Min(colisionBox.Bottom, mesh.Vertices[i * 8 + 1]);
                    colisionBox.Top = Math.Max(colisionBox.Top, mesh.Vertices[i * 8 + 1]);
                    colisionBox.Back = Math.Min(colisionBox.Back, mesh.Vertices[i * 8 + 2]);
                    colisionBox.Front = Math.Max(colisionBox.Front, mesh.Vertices[i * 8 + 2]);
                }
            }
            _colisionBoxes.Add(colisionBox);
        }

        public bool IsColiding(int index1, Vector3D<float> position1, int index2, Vector3D<float> position2)
        {
            SimpleColisionBox box1 = _colisionBoxes[index1];
            SimpleColisionBox box2 = _colisionBoxes[index2];

            return position1.X + box1.Left < position2.X + box2.Right &&
                position1.X + box1.Right > position2.X + box2.Left &&
                position1.Y + box1.Bottom < position2.Y + box2.Top &&
                position1.Y + box1.Top > position2.Y + box2.Bottom &&
                position1.Z + box1.Back < position2.Z + box2.Front &&
                position1.Z + box1.Front > position2.Z + box2.Back;
        }

        public bool DeleteModel(string path)
        {
            
            if (!_modelIndex.TryGetValue(path, out int index))
            {
                return false;
            }
            _loadedModels.ElementAt(index).DeleteModel();
            _loadedModels.RemoveAt(index);
            _modelIndex.Remove(path);
            _colisionBoxes.RemoveAt(index);
            return true;
            
        }

        public void CreateColisionBox(string path, out Vector3D<float> leftBottomBack, out Vector3D<float> rightTopFront)
        {
            if (!_modelIndex.TryGetValue(path, out int index))
            {
                leftBottomBack = Vector3D<float>.Zero;
                rightTopFront = Vector3D<float>.Zero;
                return;
            }
            SimpleColisionBox box = _colisionBoxes[index];
            leftBottomBack = new(box.Left, box.Bottom, box.Back);
            rightTopFront = new(box.Right, box.Top, box.Front);

        }
    }
}
