using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/JumpDecision")]
    public class CheckJumpDecision : Decision<Player> {
        public CheckJumpDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}
        
        public override bool Decide(Player player) => playerInputData.JumpInput;
    }
}
