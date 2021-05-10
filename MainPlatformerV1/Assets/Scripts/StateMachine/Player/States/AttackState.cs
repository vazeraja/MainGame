using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/AttackState")]
    public class AttackState : State<Player> {

        // private float nextAttack = 0;
        public override void OnEnter(Player player) {
            player.PlayerData.currentScore += 5;
        }
        public override void LogicUpdate(Player player) {
            Attack(player);
        }
        public override void OnExit(Player player) {
        }

        private void Attack(Player player) {

            if (player.AttackInput) {

                // nextAttack = Time.time + player.PlayerData.attackRate;

                // Debug.Log(nextAttack);

                player.Weapon.EnterWeapon();


            }

            if (player.isAnimationFinished) {
                player.Weapon.ExitWeapon();
            }
        }

    }
}
