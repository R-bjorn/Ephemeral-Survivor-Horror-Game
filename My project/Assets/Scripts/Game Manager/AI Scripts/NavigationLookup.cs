using System;
using System.Numerics;

namespace Game_Manager.AI_Scripts
{
    /// <summary>
    /// Hold data for the navigation lookup table.
    /// </summary>
    [Serializable]
    public struct NavigationLookup
    {
        /// <summary>
        /// The current or starting node.
        /// </summary>
        public Vector3 Current;
        
        /// <summary>
        /// Where the end goal of the navigation is.
        /// </summary>
        public Vector3 Goal;
        
        /// <summary>
        /// The node to move to from the current node in order to navigate towards the goal.
        /// </summary>
        public UnityEngine.Vector3 next;

        /// <summary>
        /// Create a data entry for a navigation lookup table.
        /// </summary>
        /// <param name="current">The current or starting node.</param>
        /// <param name="goal">Where the end goal of the navigation is.</param>
        /// <param name="next">The node to move to from the current node in order to navigate towards the goal.</param>
        public NavigationLookup(Vector3 current, Vector3 goal, UnityEngine.Vector3 next)
        {
            this.Current = current;
            this.Goal = goal;
            this.next = next;
        }
    }
}