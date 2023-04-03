using UnityEngine;
using UnityEngine.AI;

namespace Extra.EnemyMind
{
    public class EnemyMind : MonoBehaviour
    {
        public float detectionRange = 10f;  // The range at which the enemy detects the player
        public float chaseSpeed = 5f;       // The speed at which the enemy chases the player
        public float idleSpeed = 0f;        // The speed at which the enemy moves when not chasing
        
        private NavMeshAgent _agent;         // Reference to the NavMeshAgent component
        private Transform _player;           // Reference to the player's transform
        private bool _playerDetected;        // Flag to indicate if the player has been detected
        
        public float areaOfExplorationRange = 10f;

        // private NavMeshAgent navMeshAgent;
        private Vector3 startingPosition;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerDetected = false;
            startingPosition = transform.position;
            MoveToRandomPosition();
        }

        void Update()
        {
            var distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            
            // Depending on the detection range of the enemy, if player gets to close to the enemy, enemy starts chasing the player. 
            _playerDetected = distanceToPlayer <= detectionRange;
            if (_playerDetected)
            {
                _agent.speed = chaseSpeed;
                _agent.SetDestination(_player.position);
            }
            // If the enemy has reached its destination, move to a new random position
            else if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            {
                MoveToRandomPosition();
            }
        }

        void MoveToRandomPosition()
        {
            // Generate a random position within the area of exploration range
            Vector3 randomDirection = Random.insideUnitSphere * areaOfExplorationRange;
            randomDirection += startingPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, areaOfExplorationRange, NavMesh.AllAreas);

            // Move to the random position using the NavMeshAgent
            _agent.SetDestination(hit.position);
        }
    }
}