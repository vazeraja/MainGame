﻿using UnityEngine;

namespace Aarthificial.Reanimation.Nodes
{
    public interface IReanimatorGraphNode { }
    public abstract class ReanimatorNode : ScriptableObject {
        
        public string guid;
        public Vector2 position;
        
        public abstract TerminationNode Resolve(IReadOnlyReanimatorState previousState, ReanimatorState nextState);

        protected void AddTrace(ReanimatorState nextState)
        {
#if UNITY_EDITOR
            nextState.AddTrace(this);
#endif
        }
    }
}