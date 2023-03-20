using Game_Manager.AI_Scripts;

namespace COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombieDieState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("");
        }

        public override void Execute(Agent agent)
        {

        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("");
        }
    }
}
