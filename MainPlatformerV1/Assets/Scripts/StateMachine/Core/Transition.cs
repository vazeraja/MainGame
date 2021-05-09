using UnityEngine.Events;

namespace MainGame {

    [System.Serializable]
    public struct Transition<T> {
        public Decision<T> decision;
        public PlayerState_SO trueState;
        public PlayerState_SO falseState;

    }

}