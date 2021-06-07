using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MovingDecision")]
    public class CheckInputDecision : Decision<Player> {
        public CheckInputDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}
        
        public override bool Decide(Player player){
            return playerInputData.MovementInput.x != 0 && playerInputData.MovementInput.y == 0;
        }
    }
}
