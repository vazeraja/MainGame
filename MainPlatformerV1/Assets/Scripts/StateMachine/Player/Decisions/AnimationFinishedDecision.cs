using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AnimationFinishedDecision")]
    public class AnimationFinishedDecision : Decision<MainPlayer> {
        public AnimationFinishedDecision(PlayerInputData playerInputData) : base(playerInputData){}
        
        public override bool Decide(MainPlayer mainPlayer){
            return playerInputData.IsAnimationFinished;
        }
        
    }
}
