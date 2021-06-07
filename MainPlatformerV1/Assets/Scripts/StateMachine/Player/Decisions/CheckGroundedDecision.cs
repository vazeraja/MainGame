using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/GroundedDecision")]
    public class CheckGroundedDecision : Decision<Player> {
        public CheckGroundedDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}
        public override bool Decide(Player player){
            return player.IsGrounded;
        }

    }
}
