using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/JumpDecision")]
    public class CheckJumpDecision : Decision<MainPlayer> {
        public CheckJumpDecision(PlayerInputData playerInputData) : base(playerInputData){}
        
        public override bool Decide(MainPlayer mainPlayer) => playerInputData.JumpInput;
    }
}
