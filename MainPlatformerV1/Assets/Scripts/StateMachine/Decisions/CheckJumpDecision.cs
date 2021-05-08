using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/JumpDecision")]
    public class CheckJumpDecision : Decision {
        public override bool Decide(Player player) {

            bool isMoving = CheckInput(player);
            return isMoving;
        }
        private bool CheckInput(Player player) {

            if (player.JumpInput) {
                return true;
            } else {
                return false;
            }
        }
    }
}
