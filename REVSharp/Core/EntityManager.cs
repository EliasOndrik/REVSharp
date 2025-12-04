namespace REVSharp.Core
{
    internal class EntityManager
    {
        private static readonly Queue<uint> availableEntityIds = new();
        private const int MaxEntityCount = 100000;
        public EntityManager()
        {
            for (uint i = 1; i <= MaxEntityCount; i++)
            {
                availableEntityIds.Enqueue(i);
            }
        }
        public static uint IntializeEntity() => availableEntityIds.Dequeue();
        public void DestroyEntity(Entity entity) 
        {
            availableEntityIds.Enqueue(entity.Id);
            entity.Id = 0;
        }
    }
}
