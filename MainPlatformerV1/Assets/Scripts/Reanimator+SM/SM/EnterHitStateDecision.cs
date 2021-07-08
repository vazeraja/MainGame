using UnityEngine;


[CreateAssetMenu(fileName = "EnterHitStateDecision", menuName = "PluggableAI/Decisions/EnterHitStateDecision")]
public class EnterHitStateDecision : Decision {
    public override bool Decide() {
        return player.State == SasukeState.Hit;
    }
}