using System;
using System.Collections.Generic;
using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using SquareDinoTestTask.View.Creatures;
using UnityEngine;

namespace SquareDinoTestTask.View.Platforms {
    public class PlatformController : MonoBehaviour {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private Transform enemiesContainer;

        public Transform PlayerPosition => playerPosition;

        public delegate void OnStateChange();

        private event OnStateChange OnStateChangeEvent;

        public bool IsEmpty
            => _enemiesCount == 0;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private readonly List<EnemyView> _enemies = new List<EnemyView>();

        private int _enemiesCount;

        private void Awake() {
            _enemiesCount = enemiesContainer.transform.childCount;

            SetListners();
        }

        private void OnDestroy()
            => _trash.Dispose();

        public IDisposable SubscribeOnStateChange(OnStateChange call) {
            OnStateChangeEvent += call;

            return new ActionDisposable(() => { OnStateChangeEvent -= call; });
        }

        private void SetListners() {
            var containerTransform = enemiesContainer.transform;

            for (var i = 0; i < containerTransform.childCount; i++) {
                var enemy = containerTransform.GetChild(i).GetComponent<EnemyView>();
                _enemies.Add(enemy);

                var enemyHealthComponent = enemy.GetComponent<IDamageable>();
                if (enemyHealthComponent == null) {
                    continue;
                }

                _trash.Retain(enemyHealthComponent.SubscribeOnDead(EnemyIsDead));
            }
        }

        private void EnemyIsDead() {
            _enemiesCount -= 1;

            OnStateChangeEvent?.Invoke();
        }

        public void ActivetePlatform() {
            foreach (var enemy in _enemies) {
                enemy.Activate();
            }
        }
    }
}
