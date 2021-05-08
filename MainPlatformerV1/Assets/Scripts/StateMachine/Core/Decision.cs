using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class Decision : ScriptableObject {
        public abstract bool Decide(Player player);
    }
}
