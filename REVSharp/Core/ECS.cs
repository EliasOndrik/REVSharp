

namespace REVSharp.Core
{
    public class ECS
    {
        private readonly EntityManager entityManager;
        private readonly ComponentManager componentManager;
        private readonly BehaviourManager behaviourManager;

        public ECS() 
        {
            entityManager = new EntityManager();
            componentManager = new ComponentManager();
            behaviourManager = new BehaviourManager();
        }
        //Entity
        public Entity CreateEntity()
        {
            return entityManager.CreateEntity();
        }
        public void RemoveEntity(ref Entity entity) 
        {
            componentManager.RemoveComponents(ref entity);
            behaviourManager.EntityChangedMask(ref entity);
            entityManager.DestroyEntity(ref entity);
        }

        //Components
        public void RegisterComponent<T>() where T : struct 
        {
            componentManager.RegisterComponent<T>();
        }
        public void AddComponent<T>(ref Entity entity, T component) where T : struct 
        {
            componentManager.AddComponent(ref entity, component);
            behaviourManager.EntityChangedMask(ref entity);
        }
        public void RemoveComponent<T>(ref Entity entity) where T : struct
        {
            componentManager.RemoveComponent<T>(ref entity);
            behaviourManager.EntityChangedMask(ref entity);
        }
        public bool HasComponent<T>(ref Entity entity) where T : struct
        {
            return componentManager.HasComponent<T>(ref entity);
        }
        public ref T GetComponent<T>(ref readonly Entity entity) where T : struct
        {
            return ref componentManager.GetComponent<T>(in entity);
        }
        public uint GetComponentMask<T>() where T : struct
        {
            return componentManager.GetMask<T>();
        }

        //Systems
        public void RegisterSystem<T>(T system) where T : Behaviour 
        {
            behaviourManager.RegisterSystem(system);
        }
        public void SetSystemMask<T>(uint mask) where T : Behaviour
        {
            behaviourManager.SetMask<T>(mask);
        }
        public void UpdateSystems(double deltaTime) 
        {
            behaviourManager.UpdateSystems(deltaTime, this);
        }
    }
}
