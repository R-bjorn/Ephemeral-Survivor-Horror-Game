using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Extra.EnemyMind
{
    public class EnemyAI : MonoBehaviour
    {
        // Define the different states that the agent can be in
        private enum EnemyState {
            Idle,
            Patrol,
            Chase,
            Attack
        }

        // Private fields for the NavMesh agent, the current state, and other variables
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private EnemyState _currentState = EnemyState.Idle;
        
        // Player in the game
        private GameObject _player;
        private Vector3 PlayerPosition => _player.transform.position;
        
        // Patrol Points
        private Vector3 _patrolPoint;
        
        // Animator Parameters
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        // Helper methods to check if the player is within aggro range, attack range, or out of range
        private bool PlayerInRange => Vector3.Distance(transform.position, _player.transform.position) < 10f;
        private bool PlayerInAttackRange => Vector3.Distance(transform.position, _player.transform.position) < 2f;
        private bool PlayerOutOfRange => Vector3.Distance(transform.position, _player.transform.position) > 15f;

        // Start method to initialize the NavMesh agent
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();

            _player = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Update method to handle state transitions
        private void Update()
        {
            SwitchState();
        }

        // Transition to a new state method
        private void TransitionToState(EnemyState newState)
        {
            _currentState = newState;
            
            // Call the appropriate state method based on the new state
            switch (_currentState)
            {
                case EnemyState.Idle:
                    IdleState();
                    break;
                case EnemyState.Patrol:
                    PatrolState();
                    break;
                case EnemyState.Chase:
                    ChaseState();
                    break;
                case EnemyState.Attack:
                    AttackState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SwitchState()
        {
            switch (_currentState)
            {
                // Check for player within aggro range and transition to the chase state
                case EnemyState.Idle:
                    TransitionToState(PlayerInRange ? EnemyState.Chase : EnemyState.Idle);
                    break;
                case EnemyState.Patrol:
                    // Check for player within aggro range and transition to the chase state
                    TransitionToState(PlayerInRange ? EnemyState.Chase : EnemyState.Patrol);
                    break;
                case EnemyState.Chase:
                    // Check for player within attack range and transition to the attack state
                    // Check for player out of aggro range and transition to the patrol state
                    TransitionToState(PlayerInAttackRange ? EnemyState.Attack : PlayerOutOfRange ? EnemyState.Patrol : EnemyState.Chase);
                    break;
                case EnemyState.Attack:
                    // Check for player out of attack range and transition back to the chase state
                    TransitionToState(PlayerOutOfRange ? EnemyState.Chase : EnemyState.Attack);
                    break;
            }
        }

        // Idle state method
        private void IdleState()
        {
            _animator.SetTrigger(Idle);
            // Do nothing
        }

        // Patrol state method
        private void PatrolState()
        {
            // If the agent has reached its patrol point, get a new one
            if (!(_navMeshAgent.remainingDistance < 0.5f)) return;
            _patrolPoint = GetRandomPoint();
            _navMeshAgent.SetDestination(_patrolPoint);
            _animator.SetTrigger(Walking);
        }

        // Chase state method
        private void ChaseState()
        {
            // Find the player and set the agent's destination to the player's position
            _animator.SetTrigger(Walking);
            _animator.Play(Walking);
            _navMeshAgent.SetDestination(PlayerPosition);
        }

        // Attack state method
        private void AttackState()
        {
            // Find the player and attack them
            _animator.SetTrigger(Attacking);
            _animator.Play("Punching");
            // ...
        }
        
        private Vector3 GetRandomPoint()
        {
            // Generate a random point within a radius of 10 units from the agent's starting position
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, 10f, NavMesh.AllAreas);
            return hit.position;
        }
        
    }
}
