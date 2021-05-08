using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame {
    public abstract class State : ScriptableObject {

        public abstract void OnEnter(Player player);

        public abstract void LogicUpdate(Player player);

        public abstract void OnExit(Player player);

    }
}
