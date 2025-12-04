namespace REVSharp.Core
{
    internal class Entity
    {
        public uint Id { get;  set; }
        public uint ComponentMask { get; set; }
        public Entity() 
        {
            Id = EntityManager.IntializeEntity();
            ComponentMask = 0;
        }
        
    }
}
