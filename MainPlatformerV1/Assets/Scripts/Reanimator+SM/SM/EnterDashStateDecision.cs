using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterDashStateDecision", menuName = "PluggableAI/Decisions/EnterDashStateDecision")]
public class EnterDashStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Dash;
    }
}


