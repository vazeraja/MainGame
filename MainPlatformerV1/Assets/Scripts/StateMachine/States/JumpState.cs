using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/JumpState")]
    public class JumpState : State {
        public override void OnEnter(Player player) {
        }
        public override void LogicUpdate(Player player) {
            Jump(player);
            CheckIfShouldFlip(player);
        }
        public override void OnExit(Player player) {
        }
        private void Jump(Player player) {
            player.Anim.Play("Jump/Fall");

            player.Anim.SetFloat("yVelocity", player.yVelocity);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.MovementVelocity.x));

            if (player.JumpInput && player.IsGrounded) {
                player.yVelocity = player.PlayerData.jumpSpeed;
                // if (player.yVelocity > 0)
                // player.Anim.Play("player_jump");
            } else if (!player.JumpInput) {
                if (player.yVelocity > 0) {
                    // player.Anim.Play("player_fall");
                    player.yVelocity *= 0.5f;
                }
            }

            if (player.MovementInput != Vector2.zero) {
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
