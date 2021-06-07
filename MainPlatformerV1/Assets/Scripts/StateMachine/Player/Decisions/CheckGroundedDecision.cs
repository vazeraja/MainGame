using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/GroundedDecision")]
    public class CheckGroundedDecision : Decision<MainPlayer> {
        public CheckGroundedDecision(PlayerInputData playerInputData) : base(playerInputData){}
        public override bool Decide(MainPlayer mainPlayer){
            return mainPlayer.IsGrounded;
        }

    }
}
