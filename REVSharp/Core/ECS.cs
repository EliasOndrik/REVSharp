

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
        public void RemoveEntity(Entity entity) 
        {
            componentManager.RemoveComponents(entity);
            behaviourManager.EntityChangedMask(entity);
            entityManager.DestroyEntity(entity);
        }

        //Components
        public void RegisterComponent<T>() where T : IComponent 
        {
            componentManager.RegisterComponent<T>();
        }
        public void AddComponent<T>(Entity entity, T component) where T : IComponent 
        {
            componentManager.AddComponent(entity, component);
            behaviourManager.EntityChangedMask(entity);
        }
        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            componentManager.RemoveComponent<T>(entity);
            behaviourManager.EntityChangedMask(entity);
        }
        public bool HasComponent<T>(Entity entity) where T : IComponent
        {
            return componentManager.HasComponent<T>(entity);
        }
        public T GetComponent<T>(Entity entity) where T : IComponent
        {
            return componentManager.GetComponent<T>(entity);
        }
        public uint GetComponentMask<T>() where T : IComponent
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
