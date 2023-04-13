using UnityEngine;
using UnityEngine.AI;

namespace Extra
{
    public class MoveToPlayer : MonoBehaviour
    {
    
        [Tooltip("The agent to control.")]
        [SerializeField]
        private NavMeshAgent agent;

        GameObject _player;
    
        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            // if()
            // If the raycast hits, navigate to that position.
            agent.SetDestination(_player.transform.position);
        }
    }
}
