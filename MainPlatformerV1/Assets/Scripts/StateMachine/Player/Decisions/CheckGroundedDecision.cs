using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/GroundedDecision")]
    public class CheckGroundedDecision : Decision<Player> {
        public override bool Decide(Player player) {

            bool IsGrounded = CheckGrounded(player);
            return IsGrounded;
        }
        private bool CheckGrounded(Player player) {

            if (player.IsGrounded) {
                return true;
            } else {
                return false;
            }
        }
    }
}
