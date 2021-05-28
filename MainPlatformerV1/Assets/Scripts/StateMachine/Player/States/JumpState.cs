using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/JumpState")]
    public class JumpState : State<MainPlayer> {

        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        public override void OnEnter(MainPlayer mainPlayer){
        }
        public override void LogicUpdate(MainPlayer mainPlayer){
            Jump(mainPlayer);
            CheckIfShouldFlip(mainPlayer);
        }
        public override void OnExit(MainPlayer mainPlayer){

        }
        private void Jump(MainPlayer mainPlayer){
            mainPlayer.Anim.SetFloat(XVelocity, Mathf.Abs(mainPlayer.velocity.x));
            mainPlayer.Anim.SetFloat(YVelocity, mainPlayer.velocity.y);

            if (mainPlayer.JumpInput && mainPlayer.IsGrounded) {
                if (mainPlayer.velocity.y > 0) {
                }
                mainPlayer.velocity.y = mainPlayer.PlayerData.jumpSpeed;
            }
            else if (!mainPlayer.JumpInput) {
                if (mainPlayer.velocity.y > 0)
                    mainPlayer.velocity.y *= 0.5f;
            }

            if (mainPlayer.MovementInput != Vector2.zero)
                mainPlayer.MovementVelocity = mainPlayer.MovementInput * mainPlayer.PlayerData.movementSpeed;
        }
        private void CheckIfShouldFlip(MainPlayer mainPlayer){
            if (mainPlayer.MovementInput.x != 0 && mainPlayer.MovementInput.x != mainPlayer.FacingDirection)
                mainPlayer.Flip();
        }

    }
}
