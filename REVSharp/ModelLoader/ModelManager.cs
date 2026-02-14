using Silk.NET.OpenGL;
using Silk.NET.Assimp;

namespace REVSharp.ModelLoader
{
    public interface IModelManager
    {
        public void LoadModel(string path);

        public int GetModelIndex(string path);

        public void DrawModel(int index, IShader shader);
    }
    public class ModelManager : IModelManager
    {
        private readonly List<Model> _loadedModels;
        private readonly Dictionary<string, int> _modelIndex;
        private readonly GL _gl;
        
        public ModelManager(GL openGl)
        {
            _modelIndex = [];
            _loadedModels = [];
            _gl = openGl;
        }
        public void LoadModel(string path)
        {
            if(_modelIndex.ContainsKey(path))
            {
                return;
            }
            _loadedModels.Add(new Model(_gl, path));
            _modelIndex.Add(path, _loadedModels.Count - 1);
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
    }
}
