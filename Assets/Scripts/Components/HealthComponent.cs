using System;
using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.Components {
    public class HealthComponent : MonoBehaviour, IDamageable {
        [SerializeField] private float healthPoint = 5f;

        public float MaxHealth => _maxHealth;

        private event IDamageable.OnDead OnDeadEvent;
        private event IDamageable.OnHpChange OnHpChangeEvent;

        private bool _isDead;
        private float _maxHealth;

        private void Start() {
            _maxHealth = healthPoint;
        }

        public void TakeDamage(float damageValue) {
            if (_isDead) {
                return;
            }

            healthPoint = Math.Max(0, healthPoint - damageValue);
            OnHpChangeEvent?.Invoke(healthPoint);

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

        public IDisposable SubscribeOnHpChange(IDamageable.OnHpChange call) {
            OnHpChangeEvent += call;

            return new ActionDisposable(() => { OnHpChangeEvent -= call; });
        }
    }
}
