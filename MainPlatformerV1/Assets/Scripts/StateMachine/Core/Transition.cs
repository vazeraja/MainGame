using System;
using UnityEngine.Events;

namespace MainGame {
    
    [Serializable] public abstract class TransitionBase { }

    [Serializable]
    public class Transition<T, U> : TransitionBase {
        public Decision<T> decision;
        public U trueState;
        public U falseState;

        public bool enabled;

    }

}
