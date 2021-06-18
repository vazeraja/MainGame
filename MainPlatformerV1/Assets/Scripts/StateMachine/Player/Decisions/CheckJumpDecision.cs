using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Decisions/JumpDecision")]
    public class CheckJumpDecision : Decision<Player> {
        [SerializeField] private PlayerInputData playerInputData;

        public override bool Decide(Player player) => playerInputData.JumpInput;
    }
}
