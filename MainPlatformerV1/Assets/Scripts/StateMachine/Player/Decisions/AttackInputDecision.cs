﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
    public class AttackInputDecision : Decision<MainPlayer> {
        public AttackInputDecision(PlayerInputData playerInputData) : base(playerInputData){}
        public override bool Decide(MainPlayer mainPlayer){
            return playerInputData.AttackInput;
        }

    }
}
