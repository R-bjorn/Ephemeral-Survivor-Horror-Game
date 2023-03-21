using System.Collections.Generic;
using System.Linq;
using Game_Manager.AI_Scripts.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Manager.AI_Scripts
{
    /// <summary>
    /// Base class for all agents.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Agent : MessageComponent
    {
        /// <summary>
        /// The current move velocity if move acceleration is being used.
        /// </summary>
        protected Vector2 MoveVelocity { get; set; }

        /// <summary>
        /// The time passed since the last time the agent's mind made decisions. Use this instead of Time.DeltaTime.
        /// </summary>
        protected float DeltaTime { get; private set; }
    
        /// <summary>
        /// The state the agent is in.
        /// </summary>
        private State State { get; set; }
        
        
        /// <summary>
        /// The sensors of this agent.
        /// </summary>
        private Sensor[] Sensors { get; set; } 
        /// <summary>
        /// The actuators of this agent.
        /// </summary>
        private Actuator[] Actuators { get; set; }
        
        /// <summary>
        /// The root transform that holds the visuals for this agent used to rotate the agent towards its look target.
        /// </summary>
        private Transform Visuals { get; set; }
        
        /// <summary>
        /// The performance measure of this agent.
        /// </summary>
        private PerformanceMeasure PerformanceMeasure { get; set; }

        /// <summary>
        /// The current move velocity if move acceleration is being used as a Vector3.
        /// </summary>
        protected Vector3 MoveVelocity3 => new Vector3(MoveVelocity.x, 0, MoveVelocity.y);

    
        /// <summary>
        /// Implement movement behaviour.
        /// </summary>
        public abstract void MovementCalculations();
    
        /// <summary>
        /// Increase the time that has elapsed.
        /// </summary>
        public void IncreaseDeltaTime()
        {
            DeltaTime += Time.deltaTime;
        }
        
        /// <summary>
        /// Link the performance measure to this agent.
        /// </summary>
        private void ConfigurePerformanceMeasure()
        {
            if (PerformanceMeasure != null)
            {
                PerformanceMeasure.Agent = this;
            }
        }
        
        /// <summary>
        /// Called by the AgentManager to have the agent sense, think, and act.
        /// </summary>
        public virtual void Perform()
        {
            // if (Manager.Mind != null)
            // {
            //     Manager.Mind.Execute(this);
            // }
            // else
            // {
            //     if (Manager.CurrentlySelectedAgent == this && Mouse.current.rightButton.wasPressedThisFrame && Physics.Raycast(Manager.SelectedCamera.ScreenPointToRay(new(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0)), out RaycastHit hit, Mathf.Infinity, Manager.GroundLayers | Manager.ObstacleLayers))
            //     {
            //         StopMoving();
            //         Navigate(hit.point);
            //     }
            // }
            //
            // if (State != null)
            // {
            //     State.Execute(this);
            // }
            //
            // // Act on the actions.
            // ActIncomplete();
            //
            // // After all actions are performed, calculate the agent's new performance.
            // if (PerformanceMeasure != null)
            // {
            //     Performance = PerformanceMeasure.CalculatePerformance();
            // }
            //
            // // Reset the elapsed time for the next time this method is called.
            // DeltaTime = 0;
        }
        
        /// <summary>
        /// Set the state the agent is in.
        /// </summary>
        /// <typeparam name="T">The state to put the agent in.</typeparam>
        public void SetState<T>() where T : State
        {
            State value = Manager.GetState<T>();
            
            // If already in this state, do nothing.
            if (State == value)
            {
                return;
            }
            
            // Exit the current state.
            if (State != null)
            {
                State.Exit(this);
            }

            // Set the new state.
            State = value;

            // Enter the new state.
            if (State != null)
            {
                State.Enter(this);
            }
        }
        
        /// <summary>
        /// Setup the agent.
        /// </summary>
        public void Setup()
        {
            // Register this agent with the manager.
            Manager.AddAgent(this);
            
            // Find the performance measure.
            PerformanceMeasure = GetComponent<PerformanceMeasure>();
            if (PerformanceMeasure == null)
            {
                PerformanceMeasure = GetComponentInChildren<PerformanceMeasure>();
            }

            ConfigurePerformanceMeasure();

            // Find all attached actuators.
            List<Actuator> actuators = GetComponents<Actuator>().ToList();
            actuators.AddRange(GetComponentsInChildren<Actuator>());
            Actuators = actuators.Distinct().ToArray();
            foreach (Actuator actuator in Actuators)
            {
                actuator.Agent = this;
            }
        
            // Find all attached sensors.
            List<Sensor> sensors = GetComponents<Sensor>().ToList();
            sensors.AddRange(GetComponentsInChildren<Sensor>());
            Sensors = sensors.Distinct().ToArray();
            
            // Setup the percepts array to match the size of the sensors so each sensor can return a percepts to its index.
            foreach (Sensor sensor in Sensors)
            {
                sensor.Agent = this;
            }

            // Setup the root visuals transform for agent rotation.
            Transform[] children = GetComponentsInChildren<Transform>();
            if (children.Length == 0)
            {
                GameObject go = new GameObject("Visuals");
                Visuals = go.transform;
                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                return;
            }

            Visuals = children.FirstOrDefault(t => t.name == "Visuals");
            if (Visuals == null)
            {
                Visuals = children[0];
            }
        }
        
        protected virtual void Start()
        {
            // Setup the agent.
            Setup();
        
            // Enter its global and normal states if they are set.
            if (Manager.Mind != null)
            {
                Manager.Mind.Enter(this);
            }

            if (State != null)
            {
                State.Enter(this);
            }
        }

        /// <summary>
        /// Calculate movement for the agent.
        /// </summary>
        /// <param name="deltaTime">The elapsed time step.</param>
        protected void CalculateMoveVelocity(float deltaTime)
        {

        }
    }
}
