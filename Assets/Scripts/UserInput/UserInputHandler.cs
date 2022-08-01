﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace SquareDinoTestTask.UserInput {
    public class UserInputHandler : MonoBehaviour, PlayerInputActions.IPlayerActions {
        public Vector2 PointerPosition => _pointerPosition;

        private PlayerInputActions _playerInput;
        private Vector2 _pointerPosition;

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

        public void OnPointerPosition(InputAction.CallbackContext context) {
            _pointerPosition = context.ReadValue<Vector2>();
        }

        public void OnPointerClick(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed) {
                return;
            }
        }
    }
}
