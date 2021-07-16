using ThunderNut.StateMachine;

public class EnterDashStateDecision : Decision
{
    public SasukeController player => agent as SasukeController;

    public override bool Decide() {
        return player.State == SasukeState.Dash;
    }
}


