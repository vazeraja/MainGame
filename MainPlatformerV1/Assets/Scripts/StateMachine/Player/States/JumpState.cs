using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(menuName = "PluggableAI/State/JumpState")]
    public class JumpState : State<Player> {
        public JumpState(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData) { }

        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        public override void OnEnter(Player player) { }

        public override void LogicUpdate(Player player) {
            Jump(player);
            CheckIfShouldFlip(player);
        }

        public override void OnExit(Player player) { }

        private void Jump(Player player) {
            player.Anim.SetFloat(XVelocity, Mathf.Abs(player.velocity.x));
            player.Anim.SetFloat(YVelocity, player.velocity.y);

            if (playerInputData.JumpInput && player.IsGrounded) {
                if (player.velocity.y > 0) { }

                player.velocity.y = playerData.jumpSpeed;
            }
            else if (!playerInputData.JumpInput) {
                if (player.velocity.y > 0)
                    player.velocity.y *= 0.5f;
            }

            if (playerInputData.MovementInput != Vector2.zero)
                player.MovementVelocity = playerInputData.MovementInput * playerData.movementSpeed;
        }

        private void CheckIfShouldFlip(Player player) {
            if (playerInputData.MovementInput.x == 0 ||
                (int) playerInputData.MovementInput.x == playerData.facingDirection) return;
            playerData.facingDirection *= -1;
            player.transform.Rotate(0f, -180f, 0f);
        }
    }
}