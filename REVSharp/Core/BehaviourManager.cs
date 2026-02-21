
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
            uint id = entity.Id;
            foreach (var system in registeredSystems.Values)
            {
                int position = system.Entities.FindIndex(e => e.Id == id);
                if ((entity.ComponentMask & system.ComponentMask) == system.ComponentMask)
                {
                    if (position >= 0)
                    {
                        continue;
                    }
                    system.Entities.Add(entity);
                    
                }
                else 
                {
                    if (position < 0)
                    {
                        continue;
                    }
                    system.Entities.RemoveAt(position);
                }
            }
        }
        public void UpdateSystems(double deltaTime, ECS componentManager) 
        {
            foreach (var system in registeredSystems.Values)
            {
                system.OnUpdate(deltaTime, componentManager);
            }
        }
        public void RenderSystems(double deltaTime, ECS componentManager)
        {
            foreach (var system in registeredSystems.Values)
            {
                system.OnRender(deltaTime, componentManager);
            }
        }
    }
}
