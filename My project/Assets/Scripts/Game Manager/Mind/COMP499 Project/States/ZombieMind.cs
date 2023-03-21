using Game_Manager.AI_Scripts;
using Game_Manager.Mind.COMP499_Project.States.Zombie_Basic_Behaviour;
using UnityEngine;

namespace Game_Manager.Mind.COMP499_Project.States
{
    [CreateAssetMenu(menuName = "COMP499 Project/States/Zombie Mind", fileName = "Zombie Mind")]
    public class ZombieMind : State
    {
        public override void Enter(Agent agent)
        {
            // Initial roaming state
            if (agent is null)
                return;
            agent.SetState<ZombieRoamingState>();
        }

        public override void Execute(Agent agent)
        {
        }
    }
}
