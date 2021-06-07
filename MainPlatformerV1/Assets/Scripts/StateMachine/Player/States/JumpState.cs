using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/JumpState")]
    public class JumpState : State<MainPlayer> {
        public JumpState(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}

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

            if (playerInputData.JumpInput && mainPlayer.IsGrounded) {
                if (mainPlayer.velocity.y > 0) {
                }
                mainPlayer.velocity.y = playerData.jumpSpeed;
            }
            else if (!playerInputData.JumpInput) {
                if (mainPlayer.velocity.y > 0)
                    mainPlayer.velocity.y *= 0.5f;
            }

            if (playerInputData.MovementInput != Vector2.zero)
                mainPlayer.MovementVelocity = playerInputData.MovementInput * playerData.movementSpeed;
        }
        private void CheckIfShouldFlip(MainPlayer mainPlayer){
            if (playerInputData.MovementInput.x != 0 && (int)playerInputData.MovementInput.x != playerInputData.FacingDirection)
                mainPlayer.Flip();
        }

    }
}
