using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
    public class AttackInputDecision : Decision {
        public override bool Decide(Player player) {
            bool attackPressed = CheckAttack(player);
            return attackPressed;
        }

        private bool CheckAttack(Player player) {
            if (player.AttackInput == true) {
                return true;
            } else return false;
        }
    }
}
