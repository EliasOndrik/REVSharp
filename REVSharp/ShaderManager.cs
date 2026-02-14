using Silk.NET.OpenGL;
namespace REVSharp
{
    public interface IShaderManager
    {
        public void LoadShader(string name, string vertexShader, string fragmentShader);
        public int GetShaderIndex(string name);
        public IShader GetShader(int index);
    }
    public class ShaderManager : IShaderManager
    {
        private readonly List<IShader> _shaders;
        private readonly Dictionary<string, int> _shaderIndex;
        private readonly GL _gl;
        public ShaderManager(GL gl)
        {
            _gl = gl;
            _shaderIndex = [];
            _shaders = [];
        }
        public void LoadShader(string name, string vertexShader, string fragmentShader)
        {
            if (_shaderIndex.ContainsKey(name))
            {
                return;
            }
            _shaders.Add(new Shader(_gl, vertexShader, fragmentShader));
            _shaderIndex.Add(name, _shaders.Count - 1);
        }
        public int GetShaderIndex(string name)
        {
            return _shaderIndex.TryGetValue(name, out int value) ? value : -1;
        }
        public IShader GetShader(int index)
        {
            return _shaders[index];
        }
    }
}
