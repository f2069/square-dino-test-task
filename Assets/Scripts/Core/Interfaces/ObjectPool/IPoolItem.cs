using UnityEngine;

namespace SquareDinoTestTask.Core.Interfaces.ObjectPool {
    public interface IPoolItem {
        public void SetPool(int id, IPool pool);

        public void ReleaseFromPool(Vector3 spawnPosition);

        public void RetainInPool();
    }
}
