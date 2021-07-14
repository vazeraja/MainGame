using ThunderNut.StateMachine;

public class EnterMovementStateDecision : Decision
{
    public SasukeController player => agent as SasukeController;

    public override bool Decide() {
        return player.State == SasukeState.Movement;
    }
}


