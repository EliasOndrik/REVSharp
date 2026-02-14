using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using AMesh = Silk.NET.Assimp.Mesh;
using File = System.IO.File;
using StbImageSharp;


namespace REVSharp.ModelLoader
{
    public class Model
    {
        private readonly List<Texture> _loadedTextures;
        private readonly Assimp _assimp;
        private readonly GL _gl;
        public string Directory { get; private set; }
        public List<Mesh> Meshes { get; private set; } 
        public Model(GL gl, string path)
        {
            _gl = gl;
            Meshes = [];
            Directory = string.Empty;
            _loadedTextures = [];
            _assimp = Assimp.GetApi();
            LoadModel(path);
        }
        public void Draw(IShader shader)
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw(shader);
            }
        }
        private unsafe void LoadModel(string path)
        {
            
            Scene* scene = _assimp.ImportFile(path, (uint)PostProcessSteps.Triangulate | (uint)PostProcessSteps.FlipUVs);
            if (scene == null || scene->MFlags == (uint)SceneFlags.Incomplete || scene->MMeshes == null)
            {
                throw new Exception("Error loading model: " + _assimp.GetErrorStringS());
            }
            Directory = Path.GetDirectoryName(path) ?? string.Empty;
            ProcessNode(scene->MRootNode, scene);

        }
        private unsafe void ProcessNode(Node* node, Scene* scene)
        {
            for (int i = 0; i < node->MNumMeshes; i++)
            {
                AMesh* mesh = scene->MMeshes[node->MMeshes[i]];
                Meshes.Add(ProcessMesh(mesh, scene));
            }
            // Process nodes recursively
            for (uint i = 0; i < node->MNumChildren; i++)
            {
                ProcessNode(node->MChildren[i], scene);
            }

        }
        private unsafe Mesh ProcessMesh(AMesh* mesh, Scene* scene)
        {
            List<float> vertices = [];
            List<uint> indices = [];
            List<Texture> textures = [];
            for (uint i = 0; i < mesh->MNumVertices; i++)
            {
                // Positions
                vertices.Add(mesh->MVertices[i].X);
                vertices.Add(mesh->MVertices[i].Y);
                vertices.Add(mesh->MVertices[i].Z);
                // Normals
                vertices.Add(mesh->MNormals[i].X);
                vertices.Add(mesh->MNormals[i].Y);
                vertices.Add(mesh->MNormals[i].Z);
                
                // Texture Coordinates
                if (mesh->MTextureCoords[0] != null)
                {
                    vertices.Add(mesh->MTextureCoords[0][i].X);
                    vertices.Add(mesh->MTextureCoords[0][i].Y);
                }
                else
                {
                    vertices.Add(0.0f);
                    vertices.Add(0.0f);
                }
            }
            for (uint i = 0; i < mesh->MNumFaces; i++)
            {
                Face face = mesh->MFaces[i];
                for (uint j = 0; j < face.MNumIndices; j++)
                {
                    indices.Add(face.MIndices[j]);
                }
            }
            
            if (mesh->MMaterialIndex >= 0)
            {                
                
                Material* material = scene->MMaterials[mesh->MMaterialIndex];
                List<Texture> diffuseMaps = LoadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
                textures.AddRange(diffuseMaps);
                List<Texture> specularMaps = LoadMaterialTextures(material, TextureType.Specular, "texture_specular");
                textures.AddRange(specularMaps);
                
            }
            return new Mesh(_gl, [.. vertices], [.. indices], textures);
        }
        /*
        private unsafe List<Texture> LoadTextures()
        {

            return null;
        }*/
        private unsafe List<Texture> LoadMaterialTextures(Material* mat, TextureType type, string typeName)
        {
            List<Texture> textures = [];
            uint textureCount = _assimp.GetMaterialTextureCount(mat, type);
            for (uint i = 0; i < textureCount; i++)
            {
                AssimpString path = new();
                _assimp.GetMaterialTexture(mat, type, i, &path, null, null, null, null, null, null);
                string texturePath = path.ToString();
                bool skip = false;
                foreach (var loadedTexture in _loadedTextures)
                {
                    if (loadedTexture.Path == texturePath)
                    {
                        textures.Add(loadedTexture);
                        skip = true;
                        break;
                    }

                }
                if (!skip)
                {
                    string fullPath = Path.Combine(Directory, texturePath);
                    uint textureID = TextureFromFile(fullPath);
                    Texture texture = new()
                    {
                        Id = textureID,
                        Type = typeName,
                        Path = texturePath
                    };
                    textures.Add(texture);
                    _loadedTextures.Add(texture);
                }

            }
            return textures;
        }

        private unsafe uint TextureFromFile(string fullPath)
        {
            uint textureID = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.Texture2D, textureID);
            ImageResult image;
            using (FileStream stream = File.OpenRead(fullPath))
            {
                if (stream == null)
                {
                    throw new Exception("Failed to load texture at path: " + fullPath);
                }
                image = ImageResult.FromStream(stream);
            }

            var internalFormat = image.Comp switch
            {
                ColorComponents.RedGreenBlueAlpha => GLEnum.Rgba,
                ColorComponents.RedGreenBlue => GLEnum.Rgb,
                _ => GLEnum.Red,
            };
            _gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            fixed (byte * buffer = image.Data)_gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, (uint)image.Width, (uint)image.Height, 0, internalFormat, PixelType.UnsignedByte, buffer);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

            return textureID;
        }
    }
}
