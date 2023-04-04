using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ZombieMindTypes
{
    public class RoamingZombieMind : MonoBehaviour
    {
        public float detectionRange = 10f;
        public float areaOfExploration = 50f;

        // public float attackingRanage = 2f;
        
        private NavMeshAgent _agent;
        private Animator _animator;

        private GameObject _player;
        
        private bool _isPlayerDetected;
        private Vector3 _targetPosition;
        private Vector3 PlayerPosition => _player.transform.position;

        private bool _isMovingToTarget;
        // private static readonly int Walking = Animator.StringToHash("Walking");
        // private static readonly int Idle = Animator.StringToHash("Idle");
        // private static readonly int Attacking = Animator.StringToHash("Attacking");
        // private static readonly int Running = Animator.StringToHash("Running");

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponentInChildren<Animator>(); 
            _targetPosition = GetRandomPosition();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) <= detectionRange)
            {
                AttackPlayer();
            }
            else
            {
                MoveToTarget();
            }
        }

        private void MoveToTarget()
        {
            if (!_isMovingToTarget)
            {
                _agent.SetDestination(_targetPosition);
                _isMovingToTarget = true;
                // _animator.SetTrigger(Walking);
            }

            if (!(_agent.remainingDistance < 1f)) return;
            
            _targetPosition = GetRandomPosition();
            _isMovingToTarget = false;
            // _animator.SetTrigger(Idle);
        }

        private void AttackPlayer()
        {
            _agent.SetDestination(PlayerPosition);
            // _animator.SetTrigger(Vector3.Distance(transform.position, PlayerPosition) < attackingRanage ? Attacking : Running);
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
