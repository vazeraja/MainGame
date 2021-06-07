using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class Decision<T> : ScriptableObject {
        public PlayerInputData playerInputData;
        public PlayerData playerData;
        protected Decision(PlayerInputData playerInputData, PlayerData playerData){
            this.playerInputData = playerInputData;
            this.playerData = playerData;
        }
        public abstract bool Decide(T type);
    }
}
