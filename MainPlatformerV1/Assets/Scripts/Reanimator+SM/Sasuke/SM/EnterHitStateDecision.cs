using ThunderNut.StateMachine;
public class EnterHitStateDecision : Decision {
    
    public SasukeController player => agent as SasukeController;

    public override bool Decide() {
        return player.State == SasukeState.Hit;
    }
}