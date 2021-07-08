﻿using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(fileName = "EnterHitStateDecision", menuName = "PluggableAI/Decisions/EnterHitStateDecision")]
    public class EnterHitStateDecision : Decision {
        public override bool Decide() {
            return Player.State == SasukeState.Hit;
        }
    }
}