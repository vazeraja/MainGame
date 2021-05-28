using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MovingDecision")]
    public class CheckInputDecision : Decision<MainPlayer> {
        public override bool Decide(MainPlayer mainPlayer){
            return mainPlayer.MovementInput.x != 0 && mainPlayer.MovementInput.y == 0;
        }
    }
}
