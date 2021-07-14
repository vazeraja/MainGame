using ThunderNut.StateMachine;

public class EnterDashStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Dash;
    }
}


