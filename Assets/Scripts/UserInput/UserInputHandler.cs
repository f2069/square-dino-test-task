using System;
using SquareDinoTestTask.Core.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SquareDinoTestTask.UserInput {
    public class UserInputHandler : MonoBehaviour, PlayerInputActions.IPlayerActions {
        private PlayerInputActions _playerInput;
        private Vector2 _pointerPosition;

        public delegate void OnClick(Vector2 pointerPosition);

        private event OnClick OnPointerClickEvent;

        private void Awake() {
            _playerInput = new PlayerInputActions();

            _playerInput.Player.SetCallbacks(this);
            _playerInput.Enable();
        }

        private void OnDestroy() {
            _playerInput.Player.SetCallbacks(null);

            _playerInput.Disable();
            _playerInput.Dispose();
        }

        public void OnPointerPosition(InputAction.CallbackContext context)
            => _pointerPosition = context.ReadValue<Vector2>();

        public void OnPointerClick(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) {
                return;
            }

            OnPointerClickEvent?.Invoke(_pointerPosition);
        }

        public IDisposable SubscribeOnClick(OnClick call) {
            OnPointerClickEvent += call;

            return new ActionDisposable(() => { OnPointerClickEvent -= call; });
        }
    }
}
