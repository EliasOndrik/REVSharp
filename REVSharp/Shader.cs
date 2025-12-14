using Silk.NET.OpenGL;
using Silk.NET.Maths;
namespace REVSharp
{
    internal class Shader
    {
        private readonly uint _program;
        private readonly GL _gl;
        public Shader(GL gl, string vertexShader, string fragmentShader)
        {
            _gl = gl;
            uint vertexShaderHandle = LoadShader(ShaderType.VertexShader, vertexShader);
            uint fragmentShaderHandle = LoadShader(ShaderType.FragmentShader, fragmentShader);
            _program = _gl.CreateProgram();
            _gl.AttachShader(_program, vertexShaderHandle);
            _gl.AttachShader(_program, fragmentShaderHandle);
            _gl.LinkProgram(_program);
            _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int status);
            if (status == (int)GLEnum.False)
            {
                string infoLog = _gl.GetProgramInfoLog(_program);
                throw new Exception($"Error linking shader program: {infoLog}");
            }
            _gl.DetachShader(_program, vertexShaderHandle);
            _gl.DetachShader(_program, fragmentShaderHandle);
            _gl.DeleteShader(vertexShaderHandle);
            _gl.DeleteShader(fragmentShaderHandle);
        }
        public void Use()
        {
            _gl.UseProgram(_program);
        }
        public void SetFloat(string name, float value)
        {
            int location = _gl.GetUniformLocation(_program, name);
            if (location == -1) {
                throw new Exception($"Uniform {name} not found in shader.");
            }
            _gl.Uniform1(location, value);
        }
        public void SetInt(string name, int value)
        {
            int location = _gl.GetUniformLocation(_program, name);
            if (location == -1) {
                throw new Exception($"Uniform {name} not found in shader.");
            }
            _gl.Uniform1(location, value);
        }
        public unsafe void SetMatrix4x4(string name, Matrix4X4<float> value)
        {
            int location = _gl.GetUniformLocation(_program, name);
            if (location == -1) {
                throw new Exception($"Uniform {name} not found in shader.");
            }
            _gl.UniformMatrix4(location, 1, false, (float*)&value);
        }
        private uint LoadShader(ShaderType shaderType, string shader) 
        {
            string shaderFile = File.ReadAllText(shader);
            uint vertexShaderHandle = _gl.CreateShader(shaderType);
            _gl.ShaderSource(vertexShaderHandle, shaderFile);
            _gl.CompileShader(vertexShaderHandle);
            _gl.GetShader(vertexShaderHandle, ShaderParameterName.CompileStatus, out int status);
            if (status == (int)GLEnum.False)
            {
                string infoLog = _gl.GetShaderInfoLog(vertexShaderHandle);
                throw new Exception($"Error compiling {shaderType} shader: {infoLog}");
            }
            return vertexShaderHandle;
        }

    }
}
