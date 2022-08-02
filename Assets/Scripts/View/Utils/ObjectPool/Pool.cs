using System.Collections.Generic;
using SquareDinoTestTask.Core.Interfaces.ObjectPool;
using SquareDinoTestTask.Core.Utils;
using UnityEngine;

namespace SquareDinoTestTask.View.Utils.ObjectPool {
    public class Pool : MonoBehaviour, IPool {
        private readonly Dictionary<int, Stack<IPoolItem>> _items = new Dictionary<int, Stack<IPoolItem>>();

        private static Pool _instance;

        public static Pool Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                var go = new GameObject("----- MainPool -----");

                _instance = go.AddComponent<Pool>();

                return _instance;
            }
        }

        private Pool() {
        }

        public T Get<T>(GameObject go, Vector3 position) where T : class, IPoolItem {
            var id = go.GetInstanceID();
            var stack = RequireStack(id);

            IPoolItem poolItem;

            if (stack.Count > 0) {
                poolItem = stack.Pop();
                poolItem.ReleaseFromPool(position);

                return poolItem as T;
            }

            poolItem = SpawnUtils.Instance.Spawn(go.transform, position, _instance.transform)
                                 .GetComponent<IPoolItem>();

            poolItem.SetPool(id, this);
            poolItem.ReleaseFromPool(position);

            return poolItem as T;
        }

        public void RetainInPool(int id, IPoolItem poolItem) {
            var stack = RequireStack(id);
            poolItem.RetainInPool();

            stack.Push(poolItem);
        }

        private Stack<IPoolItem> RequireStack(int id) {
            if (_items.TryGetValue(id, out var stack)) {
                return stack;
            }

            stack = new Stack<IPoolItem>();
            _items.Add(id, stack);

            return stack;
        }
    }
}
