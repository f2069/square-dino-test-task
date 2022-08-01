using SquareDinoTestTask.Core.Disposables;
using SquareDinoTestTask.UserInput;
using UnityEngine;

namespace SquareDinoTestTask.View.Creatures {
    public class PlayerView : MonoBehaviour {
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private UserInputHandler _userInput;

        private void Awake() {
            _userInput = GetComponent<UserInputHandler>();

            _trash.Retain(_userInput.SubscribeOnClick(OnPointerClick));
        }

        private void OnDestroy()
            => _trash.Dispose();

        private void OnPointerClick(Vector2 pointerposition) {
        }
    }
}
