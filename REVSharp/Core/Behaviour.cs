
namespace REVSharp.Core
{
    public abstract class Behaviour
    {
        public List<Entity> Entities { get; set; }
        public uint ComponentMask { get; set; }
        protected Behaviour() 
        {
            Entities = [];
            ComponentMask = 0;
        }
        
        public abstract void Update(double deltaTime, ECS componentManager);
    }
}
