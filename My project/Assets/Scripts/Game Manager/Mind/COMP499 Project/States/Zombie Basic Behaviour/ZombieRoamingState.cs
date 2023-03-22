using DefaultNamespace;
using Game_Manager.AI_Scripts;
using Game_Manager.Mind.COMP499_Project.Game_Scripts;
using UnityEngine;

namespace Game_Manager.Mind.COMP499_Project.States.Zombie_Basic_Behaviour
{
    public class ZombieRoamingState : State
    {
        public override void Enter(Agent agent)
        {
            agent.Log("Nothing to do! Roaming");
        }

        public override void Execute(Agent agent)
        {
            // Checking if the agent is null
            if (agent is null)
                return;
            // if agent already has an action to move to a random position && agent is moving towards that direction, return null.
            if (agent.HasAction<Vector3>() && agent.Moving)                    
                return;
            // Random movement of the zombie character
            var randomPos = new Vector3(Random.Range(-20f, 20f), agent.transform.position.y, Random.Range(-20f, 20f));
            agent.Move(randomPos);
            agent.Act(randomPos);

            // Making a zombie variable from agent
            ZombieCharacter zombie = agent as ZombieCharacter;
            if (zombie is null)
                return;

            // After energy is low, after time interval
            // Zombie Moves to Resting State or Collecting Pickup State -> Random coin toss pickup
            if (zombie.isTired)
                zombie.SetState<ZombieRestingState>();
            
            // If zombie detects player from sensors && senses threat according to the player
            // Zombie moves to Evade from player state else Zombie moves to Pursue Player State
            if(zombie.sensedPlayer && zombie.sensedThreat)
                zombie.SetState<ZombieEvadePlayerState>();
            else
                zombie.SetState<ZombiePursuingPlayer>();

            // Zombie senses another zombie in area.
            if(zombie.sensedZombie)
                zombie.SetState<ZombiePursueZombieState>();
        }
        
        public override void Exit(Agent agent)
        {
            agent.Log("Something came up, gotta go");
        }
    }
}
