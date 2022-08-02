using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using SquareDinoTestTask.View.UI.Widget;
using UnityEngine;

namespace SquareDinoTestTask.View.UI {
    public class MobsHudController : MonoBehaviour {
        [SerializeField] private ProgressBarWidget healthBar;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private IDamageable _healthComponent;

        private void Start() {
            _healthComponent = GetComponentInParent<IDamageable>();

            if (_healthComponent == null) {
                return;
            }

            _trash.Retain(_healthComponent.SubscribeOnHpChange(OnHealthChanged));
        }

        private void OnDestroy()
            => _trash.Dispose();

        private void OnHealthChanged(float newValue)
            => healthBar.SetProgress(newValue / _healthComponent.MaxHealth);
    }
}
