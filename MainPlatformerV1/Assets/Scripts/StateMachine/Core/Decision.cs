using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class Decision<T> : ScriptableObject {
        public PlayerInputData playerInputData;
        protected Decision(PlayerInputData playerInputData){
            this.playerInputData = playerInputData;
        }
        public abstract bool Decide(T type);
    }
}
