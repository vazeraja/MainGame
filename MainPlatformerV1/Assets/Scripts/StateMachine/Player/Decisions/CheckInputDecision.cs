using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MovingDecision")]
    public class CheckInputDecision : Decision<MainPlayer> {
        public CheckInputDecision(PlayerInputData playerInputData) : base(playerInputData){}
        
        public override bool Decide(MainPlayer mainPlayer){
            return playerInputData.MovementInput.x != 0 && playerInputData.MovementInput.y == 0;
        }
    }
}
