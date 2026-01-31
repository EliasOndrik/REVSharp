
namespace REVSharp.Core
{
    internal class ComponentManager
    {
        private readonly Dictionary<string, IComponentArray> components;
        private readonly Dictionary<string, uint> componentMasks;
        private uint currentMask;
        public ComponentManager() 
        {
            components = [];
            componentMasks = [];
            currentMask = 1;
        }

        public void RegisterComponent<T>() where T : struct 
        {
            string componentName = typeof(T).Name;
            components.Add(componentName, new ComponentArray<T>(currentMask));
            componentMasks.Add(componentName, currentMask);
            currentMask <<= 1;

        }
        public void AddComponent<T>(ref Entity entity, T component) where T : struct 
        {
            string componentName = typeof(T).Name;
            if (HasComponent<T>(ref entity))
            {
                return;
            }
            ComponentArray<T> componentArray = (ComponentArray<T>)components[componentName];
            componentArray.Add(ref entity, component);
        }
        public void RemoveComponent<T>(ref Entity entity) where T : struct 
        {
            string componentName = typeof(T).Name;
            if (!HasComponent<T>(ref entity))
            {
                return;
            }
            ComponentArray<T> componentArray = (ComponentArray<T>)components[componentName];
            componentArray.Remove(ref entity);
            entity.ComponentMask ^= componentMasks[componentName];
        }
        public bool HasComponent<T>(ref Entity entity) where T : struct 
        {
            string componentName = typeof(T).Name;
            ComponentArray<T> componentArray = (ComponentArray<T>)components[componentName];
            return componentArray.Contains(ref entity);
        }
        public ref T GetComponent<T> (ref readonly Entity entity) where T : struct
        {
            string componentName = typeof(T).Name;
            ComponentArray<T> componentArray = (ComponentArray<T>)components[componentName];
            return ref componentArray.GetComponent(in entity);
        }
        public uint GetMask<T>() where T : struct 
        {
            return componentMasks[typeof(T).Name];
        }
        public void RemoveComponents(ref Entity entity)
        { 
            foreach (var mask in componentMasks)
            {
                if ((entity.ComponentMask & mask.Value) == mask.Value)
                {
                    IComponentArray componentArray = components[mask.Key];
                    componentArray.Remove(ref entity);
                }
            }
        }
    }
}
