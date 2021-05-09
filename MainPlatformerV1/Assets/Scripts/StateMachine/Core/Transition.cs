using UnityEngine.Events;

namespace MainGame {

    [System.Serializable]
    public struct Transition<T, U> {
        public Decision<T> decision;
        public U trueState;
        public U falseState;

    }

}