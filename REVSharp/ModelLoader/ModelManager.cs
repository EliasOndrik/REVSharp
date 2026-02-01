using Silk.NET.OpenGL;
using Silk.NET.Assimp;

namespace REVSharp.ModelLoader
{
    public class ModelManager
    {
        private List<Model> _loadedModels;
        private Dictionary<string, int> _modelIndex;
        private GL _gl;
        
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
            if (_modelIndex.ContainsKey(path))
            {
                return _modelIndex[path];
            }
            return -1;
        }

        public void DrawModel(int index)
        {
            if (index < 0 || index >= _loadedModels.Count)
            {
                return;
            }
            _loadedModels[index].Draw();
        }
    }
}
