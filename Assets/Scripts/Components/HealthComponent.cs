using System;
using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.Components {
    public class HealthComponent : MonoBehaviour, IDamageable {
        [SerializeField] private float healthPoint = 5f;

        private event IDamageable.OnDead OnDeadEvent;

        private bool _isDead;

        public void TakeDamage(float damageValue) {
            if (_isDead) {
                return;
            }

            healthPoint = Math.Max(0, healthPoint - damageValue);

            if (healthPoint != 0) {
                return;
            }

            _isDead = true;

            OnDeadEvent?.Invoke();
        }

        public IDisposable SubscribeOnDead(IDamageable.OnDead call) {
            OnDeadEvent += call;

            return new ActionDisposable(() => { OnDeadEvent -= call; });
        }
    }
}
