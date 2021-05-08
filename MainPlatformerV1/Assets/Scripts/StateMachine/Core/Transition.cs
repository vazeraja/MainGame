using UnityEngine.Events;

namespace MainGame {

    [System.Serializable]
    public class Transition {
        public Decision decision;
        public State_SO trueState;
        public State_SO falseState;

    }

}