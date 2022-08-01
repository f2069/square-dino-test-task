using System.Collections;
using SquareDinoTestTask.Core.Extensions;
using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.View.Weapons {
    public class BulletView : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private float damageValue = 1f;
        [SerializeField] protected float lifeTime = 3f;

        private Collider _collider;
        private Vector3 _direction = Vector3.zero;
        private Coroutine _coroutine;

        private void Awake() {
            _collider = GetComponent<Collider>();
        }

        private void Start() {
            _coroutine = StartCoroutine(SelfDestroy());
        }

        private void Update() {
            transform.Translate(_direction * speed * Time.deltaTime);
        }

        private IEnumerator SelfDestroy() {
            yield return new WaitForSeconds(lifeTime);

            // @todo pool
            Destroy(gameObject);
        }

        public void SetDirection(Vector3 direction)
            => _direction = direction.normalized;

        private void TryStopCoroutine() {
            if (_coroutine == null) {
                return;
            }

            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.gameObject.IsInLayer(targetLayers)) {
                return;
            }

            var healthComponent = other.GetComponent<IDamageable>() ?? other.GetComponentInParent<IDamageable>();
            if (healthComponent == null) {
                return;
            }

            _collider.enabled = false;
            healthComponent.TakeDamage(damageValue);
        }
    }
}
