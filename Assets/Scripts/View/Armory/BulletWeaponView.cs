using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Interfaces;
using SquareDinoTestTask.Core.Interfaces.ObjectPool;
using SquareDinoTestTask.Core.Utils;
using SquareDinoTestTask.UserInput;
using SquareDinoTestTask.View.Creatures;
using SquareDinoTestTask.View.Utils.ObjectPool;
using SquareDinoTestTask.View.Weapons;
using UnityEngine;

namespace SquareDinoTestTask.View.Armory {
    [RequireComponent(typeof(UserInputHandler))]
    public class BulletWeaponView : MonoBehaviour {
        [SerializeField] private float cooldown = .3f;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private BulletView bulletPrefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private UserInputHandler _userInput;
        private Cooldown _shotCooldown;
        private Camera _camera;
        private IPool _poolObjects;
        private PlayerView _player;

        private void Awake() {
            _camera = Camera.main;
            _player = GetComponent<PlayerView>();
            _userInput = GetComponent<UserInputHandler>();

            _shotCooldown = new Cooldown(cooldown);

            _trash.Retain(_userInput.SubscribeOnClick(OnPointerClick));

            _poolObjects = Pool.Instance;
        }

        private void OnDestroy()
            => _trash.Dispose();

        private bool CanShoot()
            => _shotCooldown.IsReady && !_player.IsMoved && _player.CanShoot;

        private void OnPointerClick(Vector2 pointerPosition) {
            if (!CanShoot()) {
                return;
            }

            var shotDirection = GetShootDirection(pointerPosition);
            if (shotDirection == Vector3.zero) {
                return;
            }

            var bulletGo = _poolObjects.Get<IBulletView>(bulletPrefab.gameObject, spawnPosition.position);

            bulletGo.SetDirection(shotDirection);

            _shotCooldown.Reset();
        }

        private Vector3 GetShootDirection(Vector2 mousePosition) {
            var spawnerPosition = spawnPosition.position;

            var clickRay = _camera.ScreenPointToRay(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    100f
                )
            );

            if (!Physics.Raycast(clickRay, out var clickRaycastHit, 100f)) {
                return Vector3.zero;
            }

            var targetRay = new Ray(
                spawnerPosition,
                clickRaycastHit.point - spawnerPosition
            );

            if (Physics.Raycast(targetRay, out var targetRaycastHit)) {
                return targetRaycastHit.point - spawnerPosition;
            }

            return Vector3.zero;
        }
    }
}
