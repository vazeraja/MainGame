using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MovingDecision")]
    public class CheckInputDecision : Decision<Player> {
        public override bool Decide(Player player) => player.MovementInput.x != 0 && player.MovementInput.y == 0;
    }
}