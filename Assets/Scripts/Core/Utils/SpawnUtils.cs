using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SquareDinoTestTask.Core.Utils {
    public class SpawnUtils : IDisposable {
        private const string DefaultContainerName = "------ Spawn ------";

        private Transform _defaultParent;

        private static SpawnUtils _instance;

        public static SpawnUtils Instance => _instance ??= new SpawnUtils();

        private SpawnUtils() {
        }

        private void InitDefaultContainer()
            => _defaultParent = new GameObject(DefaultContainerName).transform;

        public Transform Spawn(
            Transform prefab,
            Vector3 position,
            Transform parent = null
        ) {
            if (_defaultParent == null) {
                _instance.InitDefaultContainer();
            }

            parent = parent == null ? _defaultParent : parent;

            return Object.Instantiate(prefab, position, Quaternion.identity, parent);
        }

        public void Dispose()
            => _instance = null;
    }
}
