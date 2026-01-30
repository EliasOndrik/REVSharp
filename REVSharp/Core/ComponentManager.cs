
namespace REVSharp.Core
{
    internal class ComponentManager
    {
        private readonly Dictionary<string, Dictionary<uint, IComponent>> components;
        private readonly Dictionary<string, uint> componentMasks;
        private uint currentMask;
        public ComponentManager() 
        {
            components = [];
            componentMasks = [];
            currentMask = 1;
        }

        public void RegisterComponent<T>() where T : IComponent 
        {
            string componentName = typeof(T).Name;
            components.Add(componentName, []);
            componentMasks.Add(componentName, currentMask);
            currentMask <<= 1;

        }
        public void AddComponent<T>(Entity entity, T component) where T : IComponent 
        {
            string componentName = typeof(T).Name;
            if (HasComponent<T>(entity))
            {
                return;
            }
            components[componentName].Add(entity.Id, component);
            entity.ComponentMask |= componentMasks[componentName];
        }
        public void RemoveComponent<T>(Entity entity) where T : IComponent 
        {
            string componentName = typeof(T).Name;
            if (!HasComponent<T>(entity))
            {
                return;
            }
            components[componentName].Remove(entity.Id);
            entity.ComponentMask ^= componentMasks[componentName];
        }
        public bool HasComponent<T>(Entity entity) where T : IComponent 
        {
            return components[typeof(T).Name].ContainsKey(entity.Id);
        }
        public T GetComponent<T> (Entity entity) where T : IComponent
        {
            return (T)components[typeof(T).Name][entity.Id];
        }
        public uint GetMask<T>() where T : IComponent 
        {
            return componentMasks[typeof(T).Name];
        }
        public void RemoveComponents(Entity entity)
        { 
            foreach (var mask in componentMasks)
            {
                if ((entity.ComponentMask & mask.Value) == mask.Value)
                {
                    components[mask.Key].Remove(entity.Id);
                }
            }
        }
    }
}
