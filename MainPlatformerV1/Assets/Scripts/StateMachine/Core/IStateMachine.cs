public interface IStateMachine {
    State CurrentState { get; }
    State RemainState { get; }
    void TransitionToState(State nextState);
}