using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

public class EnterMovementStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Movement;
    }
}


