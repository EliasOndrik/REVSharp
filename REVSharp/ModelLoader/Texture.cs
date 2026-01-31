using REVSharp.Core;

namespace REVSharp.ModelLoader
{
    public struct Texture : IComponent
    {
        public uint Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        
    }
}
