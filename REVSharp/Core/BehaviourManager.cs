
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
        public void EntityChangedMask(ref Entity entity) 
        {
            foreach (var system in registeredSystems.Values)
            {
                if ((entity.ComponentMask & system.ComponentMask) == system.ComponentMask)
                {
                    if (system.Entities.Contains(entity))
                    {
                        return;
                    }
                    system.Entities.Add(entity);
                }
                else 
                {
                    if (!system.Entities.Contains(entity))
                    {
                        return;
                    }
                    system.Entities.Remove(entity);
                }
            }
        }
        public void UpdateSystems(double deltaTime, ECS componentManager) 
        {
            foreach (var system in registeredSystems.Values)
            {
                system.Update(deltaTime, componentManager);
            }
        }
    }
}
