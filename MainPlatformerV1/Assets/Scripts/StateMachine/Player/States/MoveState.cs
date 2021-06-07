using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(menuName = "PluggableAI/State/MoveState")]
    public class MoveState : State<Player> {
        public MoveState(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData) { }

        public override void OnEnter(Player player) { }

        public override void LogicUpdate(Player player) {
            Move(player);
            CheckIfShouldFlip(player);
        }

        public override void OnExit(Player player) { }

        private void Move(Player player) {
            if (playerInputData.MovementInput.x != 0 && player.IsGrounded)
                player.MovementVelocity = playerInputData.MovementInput * playerData.movementSpeed;
        }

        private void CheckIfShouldFlip(Player player) {
            if (playerInputData.MovementInput.x != 0 && (int) playerInputData.MovementInput.x != playerData.facingDirection) {
                playerData.facingDirection *= -1;
                player.transform.Rotate(0f, -180f, 0f);
            }
        }
    }
}