using SquareDinoTestTask.Core.Dictionares;
using SquareDinoTestTask.UserInput;
using UnityEngine;
using UnityEngine.AI;

namespace SquareDinoTestTask.View.Creatures {
    [RequireComponent(
        typeof(UserInputHandler),
        typeof(NavMeshAgent)
    )]
    public class PlayerView : MonoBehaviour {
        [SerializeField] private Animator animator;

        public bool IsMoved => _currentVelocity > .01f;
        public bool IsOnPosition => Vector3.Distance(transform.position, _destinationPoint) < .1f;

        public bool CanShoot { get; set; }

        private NavMeshAgent _navMeshAgent;
        private Vector3 _destinationPoint;

        private float _currentVelocity;

        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            _currentVelocity = _navMeshAgent.velocity.magnitude;

            animator.SetFloat(PlayerAnimatorConstants.HorizontalVelocityAnimationKey, _currentVelocity);
        }

        public void MoveToPoint(Vector3 destinationPoint) {
            _destinationPoint = destinationPoint;

            _navMeshAgent.SetDestination(_destinationPoint);
        }
    }
}
