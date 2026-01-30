namespace REVSharp.Core
{
    internal class EntityManager
    {
        private readonly Queue<uint> availableEntityIds = new();
        private const int MaxEntityCount = 100000;
        public EntityManager()
        {
            for (uint i = 1; i <= MaxEntityCount; i++)
            {
                availableEntityIds.Enqueue(i);
            }
        }
        public Entity CreateEntity()
        {
            if (availableEntityIds.Count == 0)
            {
                throw new InvalidOperationException("Maximum entity limit reached.");
            }
            uint id = availableEntityIds.Dequeue();
            return new Entity { Id = id, ComponentMask = 0 };
        }
        public void DestroyEntity(Entity entity) 
        {
            availableEntityIds.Enqueue(entity.Id);
            entity.Id = 0;
        }
    }
}
