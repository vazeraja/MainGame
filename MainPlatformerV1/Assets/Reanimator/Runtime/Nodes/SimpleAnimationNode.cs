using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Cels;
using UnityEngine;

namespace Aarthificial.Reanimation.Nodes
{
    [CreateAssetMenu(fileName = "simple_animation", menuName = "Reanimator/Simple Animation", order = 400)]
    public class SimpleAnimationNode : AnimationNode<SimpleCel>, IReanimatorGraphNode {
        public IEnumerable<SimpleCel> sprites { 
            get => cels;
            set => cels = value as SimpleCel[];
        }
    }
}