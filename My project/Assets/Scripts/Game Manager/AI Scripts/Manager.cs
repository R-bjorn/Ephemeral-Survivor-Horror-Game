using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Unity.Mathematics;

namespace Game_Manager.AI_Scripts
{
    [RequireComponent(typeof(Manager))]
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
        private List<string> _globalMessages = new List<string>(0);
    
        /// <summary>
        /// All agents in the scene.
        /// </summary>
        private List<Agent> Agents { get; set; } = new List<Agent>(0);
    
        [Header("Agents")]
        [Tooltip("The mind or global state agents are in. Initialize it with the global state to start it. If left empty the agent will have manual right-click-to-move controls.")]
        [SerializeField]
        private State mind;
    
        [Tooltip("The maximum number of messages any component can hold.")]
        [Min(0)]
        [SerializeField]
        private int maxMessages = 100;
        
        [Tooltip("How far an agent can be to a location its fleeing or evading from to declare it as reached. Set negative for none.")]
        [SerializeField]
        private float fleeAcceptableDistance = 10f;
        
        [Tooltip("How close an agent can be to a location its seeking or pursuing to declare it as reached. Set negative for none.")]
        [SerializeField]
        private float seekAcceptableDistance = 0.1f;
    
        [Tooltip("The radius of agents. This is for connecting navigation nodes to ensure enough space for movement.")]
        [Min(0)]
        [SerializeField]
        private float navigationRadius = 0.5f;
        
        [Tooltip(
            "How much height difference can there be between string pulls, set to zero for no limit.\n" +
            "Increase this value if generated paths are being generated between too high of slopes/stairs."
        )]
        [Min(0)]
        [SerializeField]
        private float pullMaxHeight;
        
        [Header("Visualization")]
        
        [Tooltip("The currently selected camera. Set this to start with that camera active. Leaving empty will default to the first camera by alphabetic order.")]
        
        [SerializeField]
        private Camera selectedCamera;
        
        [Tooltip("Which layers are obstacles that nodes cannot be placed on.")]
        [SerializeField]
        private LayerMask obstacleLayers;
        
        [Header("Navigation")]
        [Tooltip("Which layers can nodes be placed on.")]
        [SerializeField]
        private LayerMask groundLayers;
        
        /// <summary>
        /// Determine what mode messages are stored in.
        /// </summary>
        public static MessagingMode MessageMode => _singleton._messageMode;
    
        /// <summary>
        /// Which layers are obstacles that nodes cannot be placed on.
        /// </summary>
        public static LayerMask ObstacleLayers => _singleton.obstacleLayers;
        
        /// <summary>
        /// List of all navigation nodes.
        /// </summary>
        private readonly List<Vector3> _nodes = new List<Vector3>();
        
        /// <summary>
        /// How wide is the agent radius for connecting nodes to ensure enough space for movement.
        /// </summary>
        public static float NavigationRadius => _singleton.navigationRadius;
        
        /// <summary>
        
        /// How close an agent can be to a location its seeking or pursuing to declare it as reached.
        
        /// </summary>
        public static float SeekAcceptableDistance => _singleton.seekAcceptableDistance;
        
        /// <summary>
        /// The currently selected camera.
        /// </summary>
        public static Camera SelectedCamera => _singleton.selectedCamera;
        /// <summary>
        /// All agents which move during an update tick.
        /// </summary>
        private readonly List<Agent> _updateAgents = new List<Agent>();
    
        /// <summary>
        /// Which layers can nodes be placed on.
        /// </summary>
        public static LayerMask GroundLayers => _singleton.groundLayers;
        
        /// <summary>
        /// How far an agent can be to a location its fleeing or evading from to declare it as reached.
        /// </summary>
        public static float FleeAcceptableDistance => _singleton.fleeAcceptableDistance;
        
        /// <summary>
        /// The mind or global state agents are in
        /// </summary>
        public static State Mind => _singleton.mind;
    
        /// <summary>
        /// All agents which move during a fixed update tick.
        /// </summary>
        [SerializeField] private List<Agent> fixedUpdateAgents = new List<Agent>();
        
        /// <summary>
        /// How much height difference can there be between string pulls.
        /// </summary>
        public static float PullMaxHeight => _singleton.pullMaxHeight;
        
        /// <summary>
        /// All cameras in the scene.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private Camera[] _cameras = Array.Empty<Camera>();
    
        /// <summary>
        /// The singleton agent manager.
        /// </summary>
        private static Manager _singleton;
    
        /// <summary>
        /// The maximum number of messages any component can hold.
        /// </summary>
        public static int MaxMessages => _singleton.maxMessages;
    
        [Tooltip(
            "Determine what mode messages are stored in.\n" +
            "All - All messages are captured.\n" +
            "Compact - All messages are captured, but, duplicate messages that appear immediately after each other will be merged into only a single instance of the message.\n" +
            "Unique - No messages will be duplicated with the prior instance of the message being removed from its list when an identical message is added again."
        )]
        private MessagingMode _messageMode = MessagingMode.Compact;

        /// <summary>
        /// The navigation lookup table.
        /// </summary>
        private readonly NavigationLookup[] _navigationTable;
        
        
        private void Awake()
        
        {
        
            _singleton = GetComponent<Manager>();
        }
        
        /// <summary>
        /// Find the nearest node to a position.
        /// </summary>
        /// <param name="position">The position to find the nearest node to.</param>
        /// <returns></returns>
        private static Vector3 Nearest(Vector3 position)
        {
            // Order all nodes by distance to the position.
            List<Vector3> potential = _singleton._nodes.OrderBy(n => Vector3.Distance(n, position)).ToList();
            foreach (Vector3 node in potential)
            {
                // If the node is directly at the position, return it.
                if (node == position)
                {
                    return node;
                }
            
                // Otherwise if there is a line of sight to the node, return it.
                if (_singleton.navigationRadius <= 0)
                {
                    if (!Physics.Linecast(position, node, _singleton.obstacleLayers))
                    {
                        return node;
                    }
                    
                    continue;
                }

                Vector3 p1 = position;
                p1.y += _singleton.navigationRadius;
                Vector3 p2 = node;
                p2.y += _singleton.navigationRadius;
                if (!Physics.SphereCast(p1, _singleton.navigationRadius, (p2 - p1).normalized, out _, Vector3.Distance(p1, p2), _singleton.obstacleLayers))
                {
                    return node;
                }
            }

            // If no nodes are in line of sight, return the nearest node even though it is not in line of sight.
            return potential.First();
        }
        
        /// <summary>
        /// Lookup a path to take from a starting position to an end goal.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="goal">The end goal position.</param>
        /// <returns>A list of the points to move to to reach the goal destination.</returns>
        public static List<Vector3> LookupPath(Vector3 position, Vector3 goal)
        {
            // If there are no nodes in the lookup table simply return the end goal position.
            if (_singleton._nodes.Count == 0)
            {
                return new List<Vector3>() { goal };
            }
            
            // Check if there is a direct line of sight so we can skip pathing and just move directly towards the goal.
            if (_singleton.navigationRadius <= 0)
            {
                if (!Physics.Linecast(position, goal, _singleton.obstacleLayers))
                {
                    return new List<Vector3>() { goal };
                }
            }
            else
            {
                Vector3 p1 = position;
                p1.y += _singleton.navigationRadius;
                Vector3 p2 = goal;
                p2.y += _singleton.navigationRadius;
                if (!Physics.SphereCast(p1, _singleton.navigationRadius, (p2 - p1).normalized, out _, Vector3.Distance(p1, p2), _singleton.obstacleLayers))
                {
                    return new List<Vector3>() { goal };
                }
            }
        
            // Get the starting node and end nodes closest to their positions.
            Vector3 nodePosition = Nearest(position);
            Vector3 nodeGoal = Nearest(goal);

            // Add the starting position to the path.
            List<Vector3> path = new List<Vector3>() { position };
        
            // If the first node is not the same as the starting position, add it as well.
            if (nodePosition != position)
            {
                path.Add(nodePosition);
            }

            // Loop until the path is finished or the end goal cannot be reached.
            while (true)
            {
                try
                {
                    // Get the next node to move to.
                    NavigationLookup lookup = _singleton._navigationTable.First(l => Equals(l.Current, nodePosition) && Equals(l.Goal, nodeGoal));
                
                    // If the node is the goal destination, all nodes in the path have been finished so stop the loop.
                    if (lookup.next == nodeGoal)
                    {
                        break;
                    }
                
                    // Move to the next node and add it to the path.
                    nodePosition = lookup.next;
                    path.Add(nodePosition);
                }
                catch
                {
                    break;
                }
            }
        
            // Add the goal node to the path.
            path.Add(nodeGoal);
        
            // If the goal node and the goal itself are not the same, add the goal itself to the path as well.
            if (goal != nodeGoal)
            {
                path.Add(goal);
            }

            // Try to pull the string from both sides.
            StringPull(path);
            path.Reverse();
            StringPull(path);
            path.Reverse();

            return path;
        }
        
        /// <summary>
        /// Perform string pulling to shorten a path. Path list does not need to be returned, simply remove nodes from it.
        /// </summary>
        /// <param name="path">The path to shorten.</param>
        private static void StringPull(IList<Vector3> path)
        {
            // Loop through every point in the path less two as there must be at least two points in a path.
            for (int i = 0; i < path.Count - 2; i++)
            {
                // Inner loop from two points ahead of the outer loop to check if a node can be skipped.
                for (int j = i + 2; j < path.Count; j++)
                {
                    // Do not string pull for multi-level paths as these could skip over objects that require stairs.
                    /** if (math.abs(path[i].y - path[j].y) > Manager.PullMaxHeight)
                    {
                        continue;
                    }  **/
                
                    // If a node can be skipped as there is line of sight without it, remove it.
                    if (Manager.NavigationRadius <= 0)
                    {
                        if (!Physics.Linecast(path[i], path[j], Manager.ObstacleLayers))
                        {
                            path.RemoveAt(j-- - 1);
                        }
                        
                        continue;
                    }

                    Vector3 p1 = path[i];
                    p1.y += NavigationRadius;
                    Vector3 p2 = path[j];
                    p2.y += Manager.NavigationRadius;
                    if (!Physics.SphereCast(p1, Manager.NavigationRadius, (p2 - p1).normalized, out _, Vector3.Distance(p1, p2), Manager.ObstacleLayers))
                    {
                        path.RemoveAt(j-- - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Find all cameras in the scene so buttons can be setup for them.
        /// </summary>
        private static void FindCameras()
        {
            _singleton._cameras = FindObjectsOfType<Camera>().OrderBy(c => c.name).ToArray();
        }
        
        /// <summary>
        /// Setup all agents again.
        /// </summary>
        public static void RefreshAgents()
        {
            foreach (Agent agent in _singleton.Agents)
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
            switch (_singleton._messageMode)
            {
                case MessagingMode.Compact when _singleton._globalMessages.Count > 0 && _singleton._globalMessages[0] == message:
                    return;
                case MessagingMode.Unique:
                    _singleton._globalMessages = _singleton._globalMessages.Where(m => m != message).ToList();
                    break;
            }

            _singleton._globalMessages.Insert(0, message);
            if (_singleton._globalMessages.Count > MaxMessages)
            {
                _singleton._globalMessages.RemoveAt(_singleton._globalMessages.Count - 1);
            }
        }

        /// <summary>
        /// Register an agent with the agent manager.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        public static void AddAgent(Agent agent)
        {
            // Ensure the agent is only added once.
            if (_singleton.Agents.Contains(agent))
            {
                return;
            }
            
            // Add to their movement handling list.
            _singleton.Agents.Add(agent);
            switch (agent)
            {
                case TransformAgent updateAgent:
                    _singleton._updateAgents.Add(updateAgent);
                    break;
                case RigidbodyAgent fixedUpdateAgent:
                    _singleton.fixedUpdateAgents.Add(fixedUpdateAgent);
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
        private static readonly Dictionary<Type, State> RegisteredStates = new Dictionary<Type, State>();
        
        /// <summary>
        /// The currently selected agent.
        /// </summary>
        private Agent _selectedAgent;
        
        /// <summary>
        /// The currently selected agent.
        /// </summary>
        public static Agent CurrentlySelectedAgent => _singleton._selectedAgent;
        
        /// <summary>
        /// The agent which is currently thinking.
        /// </summary>
        private int _currentAgentIndex;

        public Manager(NavigationLookup[] navigationTable)
        {
            _navigationTable = navigationTable;
        }

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
                _selectedAgent = Agents[0];
            }

            if (Time.timeScale == 0) return;
            // Perform for all agents if there is no limit or only the next allowable number of agents if there is.
            if (maxAgentsPerUpdate <= 0)
            {
                // Keep as for loop and don't turn into a foreach in case agents destroy each other.
                foreach (var t in Agents)
                {
                    try
                    {
                        t.Perform();
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