using UnityEngine;
using UnityEngine.AI;

namespace Game_Manager.AI_Scripts
{
    // [CreateAssetMenu(fileName = "NavMeshState", menuName = "NavMeshState", order = 0)]
    public abstract class NavMeshState : ScriptableObject
    {
        /// <summary>
        /// Called when an agent first enters this state.
        /// </summary>
        /// <param name="agent">The agent.</param>
        public virtual void Enter(NavMeshAgent agent) { }

        /// <summary>
        /// Called when an agent is in this state.
        /// </summary>
        /// <param name="agent">The agent.</param>
        public virtual void Execute(NavMeshAgent agent) { }

        /// <summary>
        /// Called when an agent exits this state.
        /// </summary>
        /// <param name="agent">The agent.</param>
        public virtual void Exit(NavMeshAgent agent) { }

        /// <summary>
        /// Override to easily display the type of the component for easy usage in messages.
        /// </summary>
        /// <returns>Name of this type.</returns>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}