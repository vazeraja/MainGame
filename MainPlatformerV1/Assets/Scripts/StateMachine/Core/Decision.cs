using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class Decision<T> : ScriptableObject {
        public abstract bool Decide(T type);
    }
}
