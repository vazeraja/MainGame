using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterMovementStateDecision", menuName = "PluggableAI/Decisions/EnterMovementStateDecision")]
public class EnterMovementStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Movement;
    }
}


