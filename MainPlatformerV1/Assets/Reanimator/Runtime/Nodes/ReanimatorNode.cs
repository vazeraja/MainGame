using System;
using UnityEngine;

namespace Aarthificial.Reanimation.Nodes
{
    public abstract class ReanimatorNode : ScriptableObject {
        
        [HideInInspector] public string guid;
        [HideInInspector] public string nodeTitle = string.Empty;
        [HideInInspector] public Vector2 position;
        
        public abstract TerminationNode Resolve(IReadOnlyReanimatorState previousState, ReanimatorState nextState);

        protected void AddTrace(ReanimatorState nextState)
        {
#if UNITY_EDITOR
            nextState.AddTrace(this);
#endif
        }

        public virtual ReanimatorNode Copy()
        {
            return Instantiate(this);
        }
    }
}