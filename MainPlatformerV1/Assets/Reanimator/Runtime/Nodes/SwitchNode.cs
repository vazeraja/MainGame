using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Common;
using UnityEngine;

namespace Aarthificial.Reanimation.Nodes
{
    [CreateAssetMenu(fileName = "switch", menuName = "Reanimator/Switch", order = 400)]
    public class SwitchNode : ReanimatorNode, IReanimatorGraphNode
    {
        public List<ReanimatorNode> nodes = new List<ReanimatorNode>();
        public ControlDriver controlDriver = new ControlDriver();
        public DriverDictionary drivers = new DriverDictionary();
        

        public override TerminationNode Resolve(IReadOnlyReanimatorState previousState, ReanimatorState nextState)
        {
            AddTrace(nextState);
            nextState.Merge(drivers);
            return nodes[controlDriver.ResolveDriver(previousState, nextState, nodes.Count)]
                .Resolve(previousState, nextState);
        }
    }
}