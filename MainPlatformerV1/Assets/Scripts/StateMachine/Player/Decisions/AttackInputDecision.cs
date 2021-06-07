using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
    public class AttackInputDecision : Decision<Player> {
        public AttackInputDecision(PlayerInputData playerInputData, PlayerData playerData) : base(playerInputData, playerData){}
        public override bool Decide(Player player){
            return playerInputData.AttackInput;
        }

    }
}
