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
                player.MovementVelocity = player.MovementInput * player.PlayerData.movementSpeed;
            }
        }
        private void CheckIfShouldFlip(Player player) {
            if (player.MovementInput.x != 0 && (int)player.MovementInput.x != player.FacingDirection) {
                player.Flip();
            }
        }

    }

}
