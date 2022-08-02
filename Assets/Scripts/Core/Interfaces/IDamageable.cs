using System;

namespace SquareDinoTestTask.Core.Interfaces {
    public interface IDamageable {
        public float MaxHealth { get; }

        public delegate void OnDead();

        public delegate void OnHpChange(float healthPoint);

        public void TakeDamage(float damageValue);

        public IDisposable SubscribeOnDead(OnDead call);

        public IDisposable SubscribeOnHpChange(OnHpChange call);
    }
}
