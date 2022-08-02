using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.View.UI {
    public class MobsUIController : MonoBehaviour, IMobsUIController {
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private Transform target;

        private GameObject _uiPrefabInstance;
        private Camera _camera;

        private void Awake() {
            _camera = Camera.main;
        }

        private void Update() {
            target.transform.LookAt(_camera.transform, Vector3.up);
        }

        public void ShowMobUi() {
            if (uiPrefab == null || _uiPrefabInstance != null) {
                return;
            }

            _uiPrefabInstance = Instantiate(uiPrefab.gameObject, target, false);
            _uiPrefabInstance.GetComponent<Canvas>().worldCamera = _camera;
        }

        public void HideMobUi() {
            if (_uiPrefabInstance == null) {
                return;
            }

            Destroy(_uiPrefabInstance);
            _uiPrefabInstance = null;
        }
    }
}
