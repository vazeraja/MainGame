using UnityEngine;

namespace Aarthificial.Reanimation.Nodes {
    public class GraphRootNode : ReanimatorNode, IReanimatorGraphNode {
        
        public ReanimatorNode root;
        public override TerminationNode Resolve(IReadOnlyReanimatorState previousState, ReanimatorState nextState)
        {
            return null;
        }
        public override ReanimatorNode Copy()
        {
            GraphRootNode node = Instantiate(this);
            node.root = root.Copy();
            return node;
        }
    }
}