using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

	[CreateAssetMenu(menuName = "PluggableAI/State/MoveState")]
	public class MoveState : State<MainPlayer> {
		public MoveState(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}

		public override void OnEnter(MainPlayer mainPlayer){}
		public override void LogicUpdate(MainPlayer mainPlayer){
			Move(mainPlayer);
			CheckIfShouldFlip(mainPlayer);
		}
		public override void OnExit(MainPlayer mainPlayer){}
		private void Move(MainPlayer mainPlayer){
			if (playerInputData.MovementInput.x != 0 && mainPlayer.IsGrounded)
				mainPlayer.MovementVelocity = playerInputData.MovementInput * playerData.movementSpeed;
		}
		private void CheckIfShouldFlip(MainPlayer mainPlayer){
			if (playerInputData.MovementInput.x != 0 && (int)playerInputData.MovementInput.x != playerInputData.FacingDirection)
				mainPlayer.Flip();
		}

	}

}
