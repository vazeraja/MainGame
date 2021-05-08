using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MovingDecision")]
    public class CheckInputDecision : Decision {
        public override bool Decide(Player player) {

            bool isMoving = CheckInput(player);
            return isMoving;
        }
        private bool CheckInput(Player player) {

            if (player.MovementInput.x != 0) {
                return true;
            } else {
                return false;
            }
        }
    }
}