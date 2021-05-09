using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/MoveState")]
    public class MoveState : State<Player> {
        public override void OnEnter(Player player) {
        }
        public override void LogicUpdate(Player player) {
            Move(player);
            CheckIfShouldFlip(player);
        }
        public override void OnExit(Player player) {
        }
        private void Move(Player player) {
            if (player.MovementInput.x != 0 && player.IsGrounded) {
                player.Anim.Play("player_run");
                player.MovementVelocity = player.MovementInput * player.PlayerData.movementSpeed;
            }
        }
        public void CheckIfShouldFlip(Player player) {
            if (player.MovementInput.x != 0 && player.MovementInput.x != player.FacingDirection) {
                Flip(player);
            }
        }
        private void Flip(Player player) {

            player.FacingDirection *= -1;

            var scale = player.transform.localScale;
            scale.x *= -1;
            player.transform.localScale = scale;

        }
    }

}