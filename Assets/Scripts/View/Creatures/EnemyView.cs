using SquareDinoTestTask.Components;
using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using SquareDinoTestTask.View.UI;
using UnityEngine;

namespace SquareDinoTestTask.View.Creatures {
    [RequireComponent(
        typeof(HealthComponent),
        typeof(RagdollActivator),
        typeof(MobsUIController)
    )]
    public class EnemyView : MonoBehaviour {
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private IDamageable _healthComponent;
        private IMobsUIController _uiController;
        private IRagdollActivator _ragdollActivator;

        private void Awake() {
            _healthComponent = GetComponent<IDamageable>();
            _uiController = GetComponent<IMobsUIController>();
            _ragdollActivator = GetComponent<IRagdollActivator>();

            _trash.Retain(_healthComponent.SubscribeOnDead(OnCreatureDead));
        }

        private void Start() {
            // @todo
            _uiController.ShowMobUi();
        }

        private void OnDestroy()
            => _trash.Dispose();

        private void OnCreatureDead() {
            _uiController.HideMobUi();
            _ragdollActivator.SwitchRagdoll(true);
        }
    }
}
