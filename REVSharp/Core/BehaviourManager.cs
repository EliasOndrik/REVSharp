
using System.ComponentModel;

namespace REVSharp.Core
{
    internal class BehaviourManager
    {
        private readonly Dictionary<string, Behaviour> registeredSystems;

        public BehaviourManager() 
        {
            registeredSystems = [];
        }
        public void RegisterSystem<T>(T system) where T : Behaviour 
        {
            registeredSystems.Add(typeof(T).Name, system);
        }
        public void SetMask<T>(uint mask) where T : Behaviour 
        {
            registeredSystems[typeof(T).Name].ComponentMask = mask;
        }
        public void EntityChangedMask(Entity entity) 
        {
            foreach (var system in registeredSystems.Values)
            {
                if ((entity.ComponentMask & system.ComponentMask) == system.ComponentMask)
                {
                    system.Entities.Add(entity);
                }
                else 
                {
                    system.Entities.Remove(entity);
                }
            }
        }
    }
}
