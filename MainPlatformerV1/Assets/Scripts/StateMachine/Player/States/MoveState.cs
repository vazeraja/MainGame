using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/MoveState")]
    public class MoveState : State<MainPlayer> {

        public override void OnEnter(MainPlayer mainPlayer){
        }
        public override void LogicUpdate(MainPlayer mainPlayer){
            Move(mainPlayer);
            CheckIfShouldFlip(mainPlayer);
        }
        public override void OnExit(MainPlayer mainPlayer){
        }
        private void Move(MainPlayer mainPlayer){
            if (mainPlayer.MovementInput.x != 0 && mainPlayer.IsGrounded)
                mainPlayer.MovementVelocity = mainPlayer.MovementInput * mainPlayer.PlayerData.movementSpeed;
        }
        private void CheckIfShouldFlip(MainPlayer mainPlayer){
            if (mainPlayer.MovementInput.x != 0 && (int)mainPlayer.MovementInput.x != mainPlayer.FacingDirection)
                mainPlayer.Flip();
        }

    }

}
