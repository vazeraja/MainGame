using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AnimationFinishedDecision")]
    public class AnimationFinishedDecision : Decision<Player> {
        [SerializeField] private PlayerData playerData;
        public override bool Decide(Player player){
            return playerData.isAnimationFinished;
        }

    }
}
