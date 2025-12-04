
namespace REVSharp.Core
{
    internal abstract class Behaviour
    {
        public static ComponentManager? CManager { get; private set; }
        public List<Entity> Entities { get; set; }
        public uint ComponentMask { get; set; }
        protected Behaviour() 
        {
            Entities = [];
            ComponentMask = 0;
        }
        public static void CManagerInit(ComponentManager componentManager) 
        {
            CManager = componentManager;
        }
        public abstract void Update(float deltaTime);
    }
}
