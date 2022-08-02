using SquareDinoTestTask.Core.Interfaces;
using UnityEngine;

namespace SquareDinoTestTask.Components {
    public class RagdollActivator : MonoBehaviour, IRagdollActivator {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform ragdollRoot;

        private Rigidbody[] _rigidbodies;
        private CharacterJoint[] _joints;

        private void Awake() {
            _rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
            _joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        }

        private void Start()
            => SwitchRagdoll(false);

        public void SwitchRagdoll(bool ragdollState) {
            animator.enabled = !ragdollState;

            foreach (var joint in _joints) {
                joint.enableCollision = ragdollState;
            }

            foreach (var modelRigidbody in _rigidbodies) {
                if (ragdollState) {
                    modelRigidbody.isKinematic = false;
                    modelRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                } else {
                    modelRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    modelRigidbody.isKinematic = true;
                }

                modelRigidbody.useGravity = ragdollState;
                modelRigidbody.velocity = ragdollState ? Vector3.zero : modelRigidbody.velocity;
            }
        }
    }
}
