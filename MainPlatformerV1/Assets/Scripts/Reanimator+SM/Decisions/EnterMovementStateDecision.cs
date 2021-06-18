using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterMovementStateDecision", menuName = "PluggableAI/Decisions/Sasuke/EnterMovementStateDecision")]
public class EnterMovementStateDecision : Decision<SasukeController>
{
    public override bool Decide(SasukeController sasuke) {
        return sasuke.State == SasukeState.Movement;
    }
}


