using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ZombieMindTypes
{
    public class RoamingZombieMind : MonoBehaviour
    {
        public float detectionRange = 10f;
        public float areaOfExploration = 50f;

        private NavMeshAgent _agent;
        private Animator _animator;

        private bool _isPlayerDetected;
        private Vector3 _targetPosition;
        private bool _isMovingToTarget;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int Running = Animator.StringToHash("Running");

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _targetPosition = GetRandomPosition();
        }

        void Update()
        {
            if (_isPlayerDetected)
            {
                AttackPlayer();
            }
            else
            {
                MoveToTarget();
                DetectPlayer();
            }
        }

        void MoveToTarget()
        {
            if (!_isMovingToTarget)
            {
                _agent.SetDestination(_targetPosition);
                _isMovingToTarget = true;
                _animator.SetTrigger(Walking);
            }
            if (_agent.remainingDistance < 1f)
            {
                _targetPosition = GetRandomPosition();
                _isMovingToTarget = false;
                _animator.SetTrigger(Idle);
            }
        }

        private void DetectPlayer()
        {
            var colliders = Physics.OverlapSphere(transform.position, detectionRange);
            if (colliders.Any(c => c.CompareTag("Player")))
            {
                _isPlayerDetected = true;
                _animator.SetTrigger(Attacking);
            }
            else
            {
                _isPlayerDetected = false;
                // _animator.SetTrigger(Idle);
            }
        }

        private void AttackPlayer()
        {
            _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            var distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            _animator.SetTrigger(distance < 2f ? Attacking : Running);
        }

        private Vector3 GetRandomPosition()
        {
            var randomDirection = Random.insideUnitSphere * areaOfExploration;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, areaOfExploration, 1);
            return hit.position;
        }
    }
}
