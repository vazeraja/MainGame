using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame {
    public abstract class State<T> : ScriptableObject {

        public PlayerInputData playerInputData;
        public PlayerData playerData;
        protected State(PlayerInputData playerInputData, PlayerData playerData){
            this.playerInputData = playerInputData;
            this.playerData = playerData;
        }

        public abstract void OnEnter(T type);

        public abstract void LogicUpdate(T type);

        public abstract void OnExit(T type);

    }
}
