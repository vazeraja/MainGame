using System;
using UnityEngine;

namespace ThunderNut.StateMachine {
    public interface IDecision {
        bool Decide();
    }

    [Serializable]
    public abstract class Decision : IDecision {

        [HideInInspector] public SasukeController player;
        public abstract bool Decide();
    }
}