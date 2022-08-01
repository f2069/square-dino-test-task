using System;

namespace SquareDinoTestTask.Core.Interfaces {
    public interface IDamageable {
        public delegate void OnDead();

        public void TakeDamage(float damageValue);

        public IDisposable SubscribeOnDead(OnDead call);
    }
}
