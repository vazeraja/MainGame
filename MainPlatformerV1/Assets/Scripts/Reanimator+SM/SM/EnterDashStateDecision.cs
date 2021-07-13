using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

public class EnterDashStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Dash;
    }
}


