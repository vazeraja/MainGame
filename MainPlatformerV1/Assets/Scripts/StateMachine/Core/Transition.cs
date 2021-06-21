using System;
using UnityEngine;

namespace MainGame {
    

    [Serializable]
    public class Transition<T, U> {
        [SerializeField] public Decision<T> decision;
        [SerializeField] public U trueState;
        [SerializeField] public U falseState;
    }

}
