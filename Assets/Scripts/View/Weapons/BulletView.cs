using System.Collections;
using SquareDinoTestTask.Core.Extensions;
using SquareDinoTestTask.Core.Interfaces;
using SquareDinoTestTask.Core.Interfaces.ObjectPool;
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

        private IPool _pool;
        private int _poolId;

        private void Awake() {
            _collider = GetComponent<Collider>();
            _currentTransform = transform;
        }

        private void Update() {
            _currentTransform.position += _currentTransform.forward * speed * Time.deltaTime;
        }

        private IEnumerator SelfDestroy() {
            yield return new WaitForSeconds(lifeTime);

            _pool.RetainInPool(_poolId, this);
        }

        public void SetDirection(Vector3 direction) {
            direction.Normalize();

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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

            _pool.RetainInPool(_poolId, this);

            if (!other.gameObject.IsInLayer(targetLayers)) {
                return;
            }

            var healthComponent = other.GetComponent<IDamageable>() ?? other.GetComponentInParent<IDamageable>();
            healthComponent?.TakeDamage(damageValue);
        }

        public void SetPool(int id, IPool pool) {
            _poolId = id;
            _pool = pool;
        }

        public void ReleaseFromPool(Vector3 spawnPosition) {
            transform.position = spawnPosition;

            _collider.enabled = true;
            gameObject.SetActive(true);
            _coroutine = StartCoroutine(SelfDestroy());
        }

        public void RetainInPool() {
            _collider.enabled = false;
            TryStopCoroutine();
            gameObject.SetActive(false);
        }
    }
}
