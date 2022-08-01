using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Utils;
using SquareDinoTestTask.UserInput;
using SquareDinoTestTask.View.Weapons;
using UnityEngine;

namespace SquareDinoTestTask.View.Armory {
    public class BulletWeaponView : MonoBehaviour {
        [SerializeField] private float cooldown = .3f;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private BulletView bulletPrefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private UserInputHandler _userInput;
        private Cooldown _shotCooldown;

        private void Awake() {
            _userInput = GetComponent<UserInputHandler>();

            _shotCooldown = new Cooldown(cooldown);

            _userInput.SubscribeOnClick(OnPointerClick);
        }

        private void OnDestroy()
            => _trash.Dispose();

        private void OnPointerClick(Vector2 pointerposition) {
            if (!_shotCooldown.IsReady) {
                return;
            }

            var bulletGo = SpawnUtils.Instance.Spawn(bulletPrefab.transform, spawnPosition.position)
                                     .GetComponent<BulletView>();

            bulletGo.SetDirection(Vector3.forward);

            _shotCooldown.Reset();
        }
    }
}
