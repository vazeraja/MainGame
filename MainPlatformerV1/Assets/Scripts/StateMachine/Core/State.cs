using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainGame {
    public abstract class State<T> : ScriptableObject {
        
        public abstract void OnEnter(T type);

        public abstract void LogicUpdate(T type);
        public abstract void PhysicsUpdate(T type);

        public abstract void OnExit(T type);

    }
}
