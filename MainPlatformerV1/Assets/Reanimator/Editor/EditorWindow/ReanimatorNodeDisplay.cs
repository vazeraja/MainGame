using Aarthificial.Reanimation.Nodes;
using UnityEngine;

namespace Aarthificial.Reanimation.Editor {
    
    public sealed class ReanimatorNodeDisplay : UnityEditor.Experimental.GraphView.Node {

        public ReanimatorNode node;
        
        public ReanimatorNodeDisplay(ReanimatorNode node)
        {
            this.node = node;
            title = node.name;
        }
    }
}