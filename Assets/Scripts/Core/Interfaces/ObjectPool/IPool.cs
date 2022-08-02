using UnityEngine;

namespace SquareDinoTestTask.Core.Interfaces.ObjectPool {
    public interface IPool {
        public T Get<T>(GameObject go, Vector3 position) where T : class, IPoolItem;

        public void RetainInPool(int id, IPoolItem poolItem);
    }
}
