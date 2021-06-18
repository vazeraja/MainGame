using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterDashStateDecision", menuName = "PluggableAI/Decisions/Sasuke/EnterDashStateDecision")]
public class EnterDashStateDecision : Decision<SasukeController>
{
    public override bool Decide(SasukeController sasuke) {
        return sasuke.State == SasukeState.Dash;
    }
}


