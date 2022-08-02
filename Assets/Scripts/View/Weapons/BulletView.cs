using System.Collections;
using SquareDinoTestTask.Core.Extensions;
using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.View.Weapons {
    [RequireComponent(typeof(Collider))]
    public class BulletView : MonoBehaviour, IBulletView {
        [SerializeField] private float speed = 5f;
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private LayerMask ignoreLayers;
        [SerializeField] private float damageValue = 1f;
        [SerializeField] protected float lifeTime = 3f;

        private Collider _collider;
        private Coroutine _coroutine;
        private Transform _currentTransform;

        private void Awake() {
            _collider = GetComponent<Collider>();
            _currentTransform = transform;
        }

        private void Start() {
            _coroutine = StartCoroutine(SelfDestroy());
        }

        private void Update() {
            _currentTransform.position += _currentTransform.forward * speed * Time.deltaTime;
        }

        private IEnumerator SelfDestroy() {
            yield return new WaitForSeconds(lifeTime);

            // @todo pool
            Destroy(gameObject);
        }

        public void SetDirection(Vector3 direction) {
            direction.Normalize();

            transform.LookAt(transform.position + direction, Vector3.up);
        }

        private void TryStopCoroutine() {
            if (_coroutine == null) {
                return;
            }

            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.IsInLayer(ignoreLayers)) {
                return;
            }

            _collider.enabled = false;
            TryStopCoroutine();

            if (other.gameObject.IsInLayer(targetLayers)) {
                var healthComponent = other.GetComponent<IDamageable>() ?? other.GetComponentInParent<IDamageable>();
                healthComponent?.TakeDamage(damageValue);
            }

            // @todo pool
            Destroy(gameObject);
        }
    }
}
