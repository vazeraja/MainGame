using System;
using System.Collections.Generic;
using Aarthificial.Reanimation.ResolutionGraph.Editor;

namespace Aarthificial.Reanimation.Nodes {
    
    [Serializable]
    public class SaveData {
        public List<GroupBlock> groupBlocks = new List<GroupBlock>();
    }
    
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