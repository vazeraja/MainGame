using UnityEngine;

namespace Aarthificial.Reanimation.Nodes {
    public class GraphRootNode : ReanimatorNode, IReanimatorGraphNode {
        public override TerminationNode Resolve(IReadOnlyReanimatorState previousState, ReanimatorState nextState)
        {
            return null;
        }
    }
}