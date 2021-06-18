using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
    public class AttackInputDecision : Decision<Player> {
        [SerializeField] private PlayerInputData playerInputData;
        public override bool Decide(Player player){
            return playerInputData.AttackInput;
        }

    }
}
