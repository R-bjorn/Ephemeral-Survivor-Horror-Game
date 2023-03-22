using Game_Manager.AI_Scripts.Navigation.Nodes;

namespace Game_Manager.AI_Scripts.Navigation.Generators
{
    /// <summary>
    /// Class to implement node placing behaviours for use with a node area.
    /// </summary>
    public abstract class NodeGenerator : NodeBase
    {
        /// <summary>
        /// The node area this is attached to.
        /// </summary>
        public NodeArea NodeArea { get; set; }

        /// <summary>
        /// Generate nodes.
        /// </summary>
        public abstract void Generate();
    }
}