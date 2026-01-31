using Silk.NET.OpenGL;
using Silk.NET.Assimp;

namespace REVSharp.ModelLoader
{
    internal class ModelManager
    {
        private Dictionary<string, Model> _loadedModels;
        private GL _gl;
        private Assimp _assimp;
        
        public ModelManager(GL openGl)
        {
            _loadedModels = [];
            _gl = openGl;
            _assimp = Assimp.GetApi();
        }
        public void LoadModel(string path)
        {

        }

    }
}
