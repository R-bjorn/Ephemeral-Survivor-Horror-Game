using UnityEngine;

namespace Game_Manager.AI_Scripts.Utility
{
    /// <summary>
    /// Base component for sensors, actuators, minds, and performance measures.
    /// </summary>
    public abstract class IntelligenceComponent : MessageComponent
    {
        /// <summary>
        /// The agent this component is connected to.
        /// </summary>
        public Agent Agent { get; set; }

        /// <summary>
        /// The type of this intelligence component.
        /// </summary>
        public abstract IntelligenceComponentType Type { get; }

        protected virtual void Awake()
        {
            // Find the agent and connect to it.
            Agent = GetComponentInParent<Agent>();
            if (Agent == null)
            {
                Debug.LogError($"{GetType().Name} must be attached to an agent!");
            }
        }

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void OnEnable()
        {
            Initialize();
        }

        protected virtual void OnDisable()
        {
            // Clean up any resources.
            Cleanup();
        }

        protected virtual void OnDestroy()
        {
            // Clean up any resources.
            Cleanup();
        }

        /// <summary>
        /// Initialize this intelligence component.
        /// </summary>
        protected virtual void Initialize()
        {
            // Override this method to perform any initialization.
        }

        /// <summary>
        /// Clean up any resources used by this intelligence component.
        /// </summary>
        protected virtual void Cleanup()
        {
            // Override this method to perform any clean up.
        }

        /// <summary>
        /// Update the state of this intelligence component.
        /// </summary>
        public virtual void UpdateComponent()
        {
            // Override this method to update the component.
        }
    }

    /// <summary>
    /// The types of intelligence components.
    /// </summary>
    public enum IntelligenceComponentType
    {
        Sensor,
        Actuator,
        Mind,
        PerformanceMeasure
    }
}
