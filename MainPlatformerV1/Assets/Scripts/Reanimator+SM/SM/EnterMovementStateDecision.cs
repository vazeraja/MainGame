using ThunderNut.StateMachine;

public class EnterMovementStateDecision : Decision
{
    public override bool Decide() {
        return player.State == SasukeState.Movement;
    }
}


