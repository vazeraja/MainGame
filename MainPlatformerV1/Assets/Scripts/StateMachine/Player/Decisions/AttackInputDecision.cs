using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
    public class AttackInputDecision : Decision<Player> {
        public override bool Decide(Player player) => player.AttackInput;

    }
}
