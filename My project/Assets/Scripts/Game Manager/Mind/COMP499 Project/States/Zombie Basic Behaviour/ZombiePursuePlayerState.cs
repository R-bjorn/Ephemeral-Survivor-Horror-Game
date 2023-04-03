using Game_Manager.AI_Scripts;
using UnityEngine.AI;

namespace Game_Manager.Mind.COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombiePursuingPlayer : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Pursing player.");
        }

        public override void Execute(Agent agent)
        {
            if (agent is null)
                return;
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("No Longer pursuing player");
        }
    }
}
