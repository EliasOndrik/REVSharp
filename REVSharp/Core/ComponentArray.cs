

namespace REVSharp.Core
{
    public interface IComponentArray
    {
        public bool Add(ref Entity entity, IComponent component);
        public bool Remove(ref Entity entity);
        public bool Contains(ref Entity entity);

    }
    internal class ComponentArray<T> : IComponentArray where T : struct
    {
        private T[] _components;
        private readonly uint[][] _sparseIndex;
        private uint[] _denseEntities;
        private int _size;
        private readonly int _capacity;
        private readonly int _resize;
        private readonly uint _mask;
        public ComponentArray(uint mask)
        {
            _size = 0;
            _mask = mask;
            _capacity = 1024;
            _resize = 2;
            _sparseIndex = new uint[(EntityManager.MaxEntityCount / _capacity) + 1][];
            _denseEntities = new uint[_capacity];
            _components = new T[_capacity];
        }

        public bool Add(ref Entity entity, IComponent component)
        {
            return Add(ref entity, (T)component);
        }
        public bool Add(ref Entity entity, T component)
        {
            if (Contains(ref entity))
            {
                return false;
            }
            if (_size >= _denseEntities.Length)
            {
                int newCapacity = _denseEntities.Length * _resize;
                Array.Resize(ref _denseEntities, newCapacity);
                newCapacity = _components.Length * _resize;
                Array.Resize(ref _components, newCapacity);
            }
            var indexInfo = GetSparseInfo(entity.Id, _capacity);
            if (_sparseIndex[indexInfo.Item1] == null)
            {
                _sparseIndex[indexInfo.Item1] = new uint[_capacity];
            }
            _sparseIndex[indexInfo.Item1][indexInfo.Item2] = (uint)_size;
            _denseEntities[_size] = entity.Id;
            _components[_size] = component;
            _size++;
            entity.ComponentMask |= _mask;
            return true;
        }

        public bool Contains(ref Entity entity)
        {
            var indexInfo = GetSparseInfo(entity.Id, _capacity);
            if (_sparseIndex.Length <= indexInfo.Item1 || _sparseIndex[indexInfo.Item1] == null)
            {
                return false;
            }
            if (_denseEntities[_sparseIndex[indexInfo.Item1][indexInfo.Item2]] != entity.Id)
            {
                return false;
            }
            return true;
        }

        public bool Remove(ref Entity entity)
        {
            if (!Contains(ref entity))
            {
                return false;
            }
            if (_denseEntities[_size - 1] == entity.Id)
            {
                _size--;
                return true;
            }
            var indexInfo = GetSparseInfo(entity.Id, _capacity);
            uint removeIndex = _sparseIndex[indexInfo.Item1][indexInfo.Item2];
            uint lastEntityId = _denseEntities[_size - 1];
            var lastIndexInfo = GetSparseInfo(lastEntityId, _capacity);
            _components[removeIndex] = _components[_size - 1];
            _denseEntities[removeIndex] = lastEntityId;
            _sparseIndex[lastIndexInfo.Item1][lastIndexInfo.Item2] = removeIndex;
            _size--;
            entity.ComponentMask ^= _mask;
            return true;
        }
        public ref T GetComponent(ref readonly Entity entity)
        {
            var indexInfo = GetSparseInfo(entity.Id, _capacity);
            return ref _components[_sparseIndex[indexInfo.Item1][indexInfo.Item2]];
        }

        private static Tuple<int, int> GetSparseInfo(uint entityId, int capacity)
        {
            int page = (int)(entityId / capacity);
            int index = (int)(entityId % capacity);
            return Tuple.Create(page, index);
        }

    }
}
