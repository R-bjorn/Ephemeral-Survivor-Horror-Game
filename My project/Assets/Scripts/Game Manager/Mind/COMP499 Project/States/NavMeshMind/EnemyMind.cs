using Game_Manager.AI_Scripts;
using Game_Manager.Mind.COMP499_Project.States.Zombie_Basic_Behaviour;
using UnityEngine;
using UnityEngine.AI;

namespace Game_Manager.Mind.COMP499_Project.States.NavMeshMind
{
    public class EnemyMind : NavMeshState
    {
        public override void Enter(NavMeshAgent agent)
        {
            // // Initial roaming state
            if (agent is null)
                return;
            // agent.SetState<ZombiePursuingPlayer>();
        }

        public override void Execute(NavMeshAgent agent)
        {
        }
    }
}