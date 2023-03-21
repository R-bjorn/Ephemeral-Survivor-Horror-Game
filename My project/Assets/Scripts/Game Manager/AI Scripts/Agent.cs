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
        /// Class to store all targets the agent is moving in relation to.
        /// </summary>
        private abstract class Movement
        {
            /// <summary>
            /// The transform to move in relation to.
            /// </summary>
            private readonly Transform _transform;

            /// <summary>
            /// True if this move data was setup with a transform so if at any point the transform is destroyed this is removed as well.
            /// </summary>
#pragma warning disable 414
            private readonly bool _isTransformTarget;
#pragma warning restore 414
            
            /// <summary>
            /// Store the position which is only used if the transform is null.
            /// </summary>
            private readonly Vector2 _position;

            /// <summary>
            /// How much time has elapsed since the last time this was called for predictive move types.
            /// </summary>
            private float _deltaTime;

            /// <summary>
            /// The movement vector for visualizing move data.
            /// </summary>
            public Vector2 MoveVector = Vector2.zero;
        
            /// <summary>
            /// Get the position of the transform if it has one otherwise the position it was set to have.
            /// </summary>
            public Vector2 Position
            {
                get
                {
                    if (_transform == null)
                    {
                        return _position;
                    }

                    var pos3 = _transform.position;
                    return new Vector2(pos3.x, pos3.z);
                }
            }

            /// <summary>
            /// Create a move data for a transform.
            /// </summary>
            /// <param name="behaviour">The move type.</param>
            /// <param name="transform">The transform.</param>
            protected Movement(Steering.Behaviour behaviour, Transform transform, float deltaTime)
            {
                _transform = transform;
                _deltaTime = deltaTime;
                var pos3 = transform.position;
                _position = new Vector2(pos3.x, pos3.z);
                _isTransformTarget = true;
            }

            /// <summary>
            /// Create a move data for a position.
            /// </summary>
            /// <param name="behaviour">The move type.</param>
            /// <param name="position">The position.</param>
            protected Movement(Steering.Behaviour behaviour, Vector2 position, float deltaTime)
            {
                // Since pursuit and evade are for moving objects and this is only with a static position,
                // switch pursuit to seek and evade to flee.
                behaviour = behaviour switch
                {
                    Steering.Behaviour.Pursue => Steering.Behaviour.Seek,
                    Steering.Behaviour.Evade => Steering.Behaviour.Flee,
                    _ => behaviour
                };

                _transform = null;
                _position = position;
                _deltaTime = deltaTime;
                _isTransformTarget = false;
            }
        }

        /// <summary>
        /// The actions of this agent that are not yet completed.
        /// </summary>
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly List<object> _inProgressActions = new List<object>();

        protected Agent(Vector2 moveVelocity)
        {
            MoveVelocity = moveVelocity;
        }

        /// <summary>
        /// The current path an agent is following.
        /// </summary>
        private List<Vector3> Path { get; set; } = new List<Vector3>();

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
        /// All movement the agent is doing without path finding.
        /// </summary>
        // ReSharper disable once CollectionNeverUpdated.Local
        private List<Movement> Moves { get; } = new List<Movement>();

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
        /// The path destination.
        /// </summary>
        private Vector3? Destination => Path.Count > 0 ? Path[-1] : (Vector3?)null;
    
        
        
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
        /// Clear all move data.
        /// </summary>
        private void StopMoving()
        {
            Moves.Clear();
        }

        /// <summary>
        /// Calculate a path towards a position.
        /// </summary>
        /// <param name="goal">The position to navigate to.</param>
        /// <returns>True if the path has been set, false if the agent was already navigating towards this point.</returns>
        private void Navigate(Vector3 goal)
        {
            if (Destination == goal)
            {
                return;
            }
        
            Path = Manager.LookupPath(transform.position, goal);
        }
        
        /// <summary>
        /// Called by the AgentManager to have the agent sense, think, and act.
        /// </summary>
        public virtual void Perform()
        {
            if (Manager.Mind != null)
            {
                Manager.Mind.Execute(this);
            }
            else
            {
                if (Manager.CurrentlySelectedAgent == this && Mouse.current.rightButton.wasPressedThisFrame && Physics.Raycast(Manager.SelectedCamera.ScreenPointToRay(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0)), out RaycastHit hit, Mathf.Infinity, Manager.GroundLayers | Manager.ObstacleLayers))
                {
                    StopMoving();
                    Navigate(hit.point);
                }
            }
            
            if (State != null)
            {
                State.Execute(this);
            }
            
            // Act on the actions.
            ActIncomplete();
            
            // After all actions are performed, calculate the agent's new performance.
            if (PerformanceMeasure != null)
            {
                PerformanceMeasure.CalculatePerformance();
            }
            
            // Reset the elapsed time for the next time this method is called.
            DeltaTime = 0;
        }
        
        /// <summary>
        /// Perform actions that are still incomplete.
        /// </summary>
        private void ActIncomplete()
        {
            for (int i = 0; i < _inProgressActions.Count; i++)
            {
                bool completed = false;
                
                foreach (Actuator actuator in Actuators)
                {
                    completed = actuator.Act(_inProgressActions[i]);
                    if (completed)
                    {
                        break;
                    }
                }

                if (!completed)
                {
                    continue;
                }

                _inProgressActions.RemoveAt(i--);
            }
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
