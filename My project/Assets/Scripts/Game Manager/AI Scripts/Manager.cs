using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Manager.AI_Scripts
{
    public class Manager : MonoBehaviour
    {
        /// <summary>
        /// Determine what mode messages are stored in.
        /// All - All messages are captured.
        /// Compact - All messages are captured, but, duplicate messages that appear immediately after each other will be merged into only a single instance of the message.
        /// Unique - No messages will be duplicated with the prior instance of the message being removed from its list when an identical message is added again.
        /// </summary>
        public enum MessagingMode : byte
        {
            All,
            Compact,
            Unique
        }
    
        /// <summary>
        /// The global messages.
        /// </summary>
        private List<string> _globalMessages = new();
    
        /// <summary>
        /// All agents in the scene.
        /// </summary>
        public List<Agent> Agents { get; private set; } = new();
    
        [Header("Agents")]
        [Tooltip("The mind or global state agents are in. Initialize it with the global state to start it. If left empty the agent will have manual right-click-to-move controls.")]
        [SerializeField]
        private State mind;
    
        [Tooltip("The maximum number of messages any component can hold.")]
        [Min(0)]
        [SerializeField]
        private int maxMessages = 100;
    
        /// <summary>
        /// Determine what mode messages are stored in.
        /// </summary>
        public static MessagingMode MessageMode => Singleton._messageMode;
    
        /// <summary>
        /// All agents which move during an update tick.
        /// </summary>
        private readonly List<Agent> _updateAgents = new();
    
        /// <summary>
        /// The mind or global state agents are in
        /// </summary>
        public static State Mind => Singleton.mind;
    
        /// <summary>
        /// All agents which move during a fixed update tick.
        /// </summary>
        private readonly List<Agent> _fixedUpdateAgents = new();
    
        /// <summary>
        /// All cameras in the scene.
        /// </summary>
        private Camera[] _cameras = Array.Empty<Camera>();
    
        /// <summary>
        /// The singleton agent manager.
        /// </summary>
        protected static Manager Singleton;
    
        /// <summary>
        /// The maximum number of messages any component can hold.
        /// </summary>
        public static int MaxMessages => Singleton.maxMessages;
    
        [Tooltip(
            "Determine what mode messages are stored in.\n" +
            "All - All messages are captured.\n" +
            "Compact - All messages are captured, but, duplicate messages that appear immediately after each other will be merged into only a single instance of the message.\n" +
            "Unique - No messages will be duplicated with the prior instance of the message being removed from its list when an identical message is added again."
        )]
        private MessagingMode _messageMode = MessagingMode.Compact;
    
        /// <summary>
        /// Find all cameras in the scene so buttons can be setup for them.
        /// </summary>
        private static void FindCameras()
        {
            Singleton._cameras = FindObjectsOfType<Camera>().OrderBy(c => c.name).ToArray();
        }
        
        /// <summary>
        /// Setup all agents again.
        /// </summary>
        public static void RefreshAgents()
        {
            foreach (Agent agent in Singleton.Agents)
            {
                agent.Setup();
            }
        }
    
        /// <summary>
        /// Add a message to the global message list.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public static void GlobalLog(string message)
        {
            switch (Singleton._messageMode)
            {
                case MessagingMode.Compact when Singleton._globalMessages.Count > 0 && Singleton._globalMessages[0] == message:
                    return;
                case MessagingMode.Unique:
                    Singleton._globalMessages = Singleton._globalMessages.Where(m => m != message).ToList();
                    break;
                case MessagingMode.All:
                default:
                    break;
            }

            Singleton._globalMessages.Insert(0, message);
            if (Singleton._globalMessages.Count > MaxMessages)
            {
                Singleton._globalMessages.RemoveAt(Singleton._globalMessages.Count - 1);
            }
        }

        /// <summary>
        /// Register an agent with the agent manager.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        public static void AddAgent(Agent agent)
        {
            // Ensure the agent is only added once.
            if (Singleton.Agents.Contains(agent))
            {
                return;
            }
            
            // Add to their movement handling list.
            Singleton.Agents.Add(agent);
            switch (agent)
            {
                case TransformAgent updateAgent:
                    Singleton._updateAgents.Add(updateAgent);
                    break;
                case RigidbodyAgent fixedUpdateAgent:
                    Singleton._fixedUpdateAgents.Add(fixedUpdateAgent);
                    break;
            }
            
            // If the agent had any cameras attached to it we need to add them.
            FindCameras();
            
            // CheckGizmos();
        }
        
        [Header("Performance")]
        [Tooltip("The maximum number of agents which can be updated in a single frame. Set to zero to be unlimited.")]
        [Min(0)]
        [SerializeField]
        private int maxAgentsPerUpdate;
        
        /// <summary>
        /// All registered states.
        /// </summary>
        private static readonly Dictionary<Type, State> RegisteredStates = new();
        
        /// <summary>
        /// The currently selected agent.
        /// </summary>
        protected Agent SelectedAgent;
        
        /// <summary>
        /// The currently selected agent.
        /// </summary>
        public static Agent CurrentlySelectedAgent => Singleton.SelectedAgent;
        
        /// <summary>
        /// The agent which is currently thinking.
        /// </summary>
        private int _currentAgentIndex;
        
        /// <summary>
        /// Go to the next agent.
        /// </summary>
        private void NextAgent()
        {
            _currentAgentIndex++;
            if (_currentAgentIndex >= Agents.Count)
            {
                _currentAgentIndex = 0;
            }
        }
        
        /// <summary>
        /// Handle moving of agents.
        /// </summary>
        /// <param name="agents">The agents to move.</param>
        private static void MoveAgents(List<Agent> agents)
        {
            foreach (Agent agent in agents)
            {
                agent.MovementCalculations();
            }
        }
        
        /// <summary>
        /// Lookup a state type from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of state to register</typeparam>
        /// <returns>The state of the requested type.</returns>
        public static State GetState<T>() where T : State
        {
            return RegisteredStates.ContainsKey(typeof(T)) ? RegisteredStates[typeof(T)] : CreateState<T>();
        }
        
        /// <summary>
        /// Create a state if there was not one within the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of state to register</typeparam>
        /// <returns>The state instance that was created</returns>
        private static State CreateState<T>() where T : State
        {
            RegisterState<T>(ScriptableObject.CreateInstance(typeof(T)) as State);
            return RegisteredStates[typeof(T)];
        }
        
        /// <summary>
        /// Register a state type into the dictionary for future reference.
        /// </summary>
        /// <param name="stateToAdd">The state itself.</param>
        /// <typeparam name="T">The type of state to register</typeparam>
        private static void RegisterState<T>(State stateToAdd) where T : State
        {
            RegisteredStates[typeof(T)] = stateToAdd;
        }
        
        protected virtual void Update()
        {
            transform.position = Vector3.zero;
            
            if (Agents.Count == 1)
            {
                SelectedAgent = Agents[0];
            }

            if (Time.timeScale != 0)
            {
                // Perform for all agents if there is no limit or only the next allowable number of agents if there is.
                if (maxAgentsPerUpdate <= 0)
                {
                    // Keep as for loop and don't turn into a foreach in case agents destroy each other.
                    for (int i = 0; i < Agents.Count; i++)
                    {
                        try
                        {
                            Agents[i].Perform();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < maxAgentsPerUpdate; i++)
                    {
                        try
                        {
                            Agents[_currentAgentIndex].Perform();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                
                        NextAgent();
                    }
                }

                // Update the delta time for all agents and look towards their targets.
                foreach (Agent agent in Agents)
                {
                    agent.IncreaseDeltaTime();
                    // agent.LookCalculations();
                }

                // Move agents that do not require physics.
                MoveAgents(_updateAgents);
            }
            

            // // Click to select an agent.
            // if (Mouse.current.leftButton.wasPressedThisFrame && Physics.Raycast(selectedCamera.ScreenPointToRay(new(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0)), out RaycastHit hit, Mathf.Infinity))
            // {
            //     // See if an agent was actually hit with the click and select it if so.
            //     Transform tr = hit.collider.transform;
            //     do
            //     {
            //         Agent clicked = tr.GetComponent<Agent>();
            //         if (clicked != null)
            //         {
            //             SelectedAgent = clicked;
            //             followBest = false;
            //             break;
            //         }
            //         tr = tr.parent;
            //     } while (tr != null);
            // }
            //
            // if (!followBest)
            // {
            //     return;
            // }
            //
            // // If locked to following the best agent, select the best agent.
            // float best = float.MinValue;
            // SelectedAgent = null;
            // foreach (Agent agent in Agents.Where(a => a.PerformanceMeasure != null))
            // {
            //     float score = agent.PerformanceMeasure.CalculatePerformance();
            //     if (score <= best)
            //     {
            //         continue;
            //     }
            //
            //     best = score;
            //     SelectedAgent = agent;
            // }
            //
            // if (SelectedAgent == null)
            // {
            //     followBest = false;
            //     return;
            // }
            //
            // if (Singleton._state == GuiState.Main)
            // {
            //     Singleton._state = GuiState.Agent;
            // }
        }
    }
}