using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AnimationFinishedDecision")]
    public class AnimationFinishedDecision : Decision {
        public override bool Decide(Player player) {
            bool isAnimationFinished = FinishTrigger(player);
            return isAnimationFinished;
        }

        private bool FinishTrigger(Player player) {
            if (player.isAnimationFinished) {
                return true;
            } else return false;
        }
    }
}
