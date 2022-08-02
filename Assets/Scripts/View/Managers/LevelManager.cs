using System.Collections.Generic;
using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.Core.Utils;
using SquareDinoTestTask.UserInput;
using SquareDinoTestTask.View.Creatures;
using SquareDinoTestTask.View.Platforms;
using UnityEngine;

namespace SquareDinoTestTask.View.Managers {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private PlayerView player;
        [SerializeField] private List<PlatformController> platforms;

        private readonly CompositeDisposable _platformsDisposable = new CompositeDisposable();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private UserInputHandler _userInput;
        private PlatformController _currentPlatform;
        private LevelLoader _levelLoader;

        private int _currentPlatformIndex;
        private bool _lastPlatform;
        private bool _isOnReload;

        private void Awake() {
            _levelLoader = new LevelLoader();
            _userInput = player.GetComponent<UserInputHandler>();

            _currentPlatform = platforms[_currentPlatformIndex];
            SubscribeOnPlatform();

            _trash.Retain(_userInput.SubscribeOnClick(OnPointerClick));
        }

        private void Update() {
            if (!_lastPlatform || _isOnReload) {
                return;
            }

            if (!player.IsOnPosition) {
                return;
            }

            _isOnReload = true;
            _levelLoader.ReloadLevel();
        }

        private void OnDestroy() {
            _platformsDisposable.Dispose();
            _trash.Dispose();

            SpawnUtils.Instance.Dispose();
        }

        private void SubscribeOnPlatform() {
            _platformsDisposable.Dispose();
            _platformsDisposable.Retain(_currentPlatform.SubscribeOnStateChange(OnPlatformStateChanged));
        }

        private void OnPlatformStateChanged() {
            if (!_currentPlatform.IsEmpty) {
                return;
            }

            MoveToNextPlatform();
        }

        private void OnPointerClick(Vector2 pointerposition) {
            if (!_currentPlatform.IsEmpty || player.IsMoved) {
                return;
            }

            MoveToNextPlatform();
        }

        private void MoveToNextPlatform() {
            if (_lastPlatform) {
                return;
            }

            if (_currentPlatformIndex < platforms.Count - 1) {
                _currentPlatformIndex += 1;
                _currentPlatform = platforms[_currentPlatformIndex];
            }

            if (_currentPlatformIndex == 1) {
                player.CanShoot = true;
            }

            if (_currentPlatformIndex == platforms.Count - 1) {
                _lastPlatform = true;
            }

            if (!_lastPlatform && _currentPlatform.IsEmpty) {
                MoveToNextPlatform();

                return;
            }

            SubscribeOnPlatform();

            player.MoveToPoint(_currentPlatform.PlayerPosition.position);
            _currentPlatform.ActivetePlatform();
        }
    }
}
