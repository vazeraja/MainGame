using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/JumpState")]
    public class JumpState : State<Player> {

        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        public override void OnEnter(Player player){
        }
        public override void LogicUpdate(Player player){
            Jump(player);
            CheckIfShouldFlip(player);
        }
        public override void OnExit(Player player){

        }
        private void Jump(Player player){
            player.Anim.SetFloat(XVelocity, Mathf.Abs(player.velocity.x));
            player.Anim.SetFloat(YVelocity, player.velocity.y);

            if (player.JumpInput && player.IsGrounded) {
                if (player.velocity.y > 0) {
                }
                player.velocity.y = player.PlayerData.jumpSpeed;
            }
            else if (!player.JumpInput) {
                if (player.velocity.y > 0)
                    player.velocity.y *= 0.5f;
            }

            if (player.MovementInput != Vector2.zero)
                player.MovementVelocity = player.MovementInput * player.PlayerData.movementSpeed;
        }
        private void CheckIfShouldFlip(Player player){
            if (player.MovementInput.x != 0 && player.MovementInput.x != player.FacingDirection)
                player.Flip();
        }

    }
}
