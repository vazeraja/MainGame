using UnityEngine;


public class EnterHitStateDecision : Decision {
    public override bool Decide() {
        return player.State == SasukeState.Hit;
    }
}