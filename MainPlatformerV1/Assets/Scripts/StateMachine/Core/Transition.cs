using System;
using UnityEngine;

namespace MainGame {
    
    [Serializable] public abstract class TransitionBase { }

    [Serializable]
    public class Transition<T, U> : TransitionBase {
        [SerializeField] public Decision<T> decision;
        [SerializeField] public U trueState;
        [SerializeField] public U falseState;
    }

}
