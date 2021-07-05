using System.Collections.Generic;
using UnityEngine;


public interface ITransition {
    void SetDecision(Decision decision);
    void SetStates(State a, State b);
    Decision GetDecision();
    List<State> GetStates();
}

[CreateAssetMenu(menuName = "PluggableAI/Transition")]
public class Transition : ScriptableObject, ITransition {
    
    public Decision decision;
    public State trueState;
    public State falseState;

    public void SetDecision(Decision d) {
        decision = d;
    }

    public void SetStates(State t, State f) {
        trueState = t;
        falseState = f;
    }

    public Decision GetDecision() => decision;
    public List<State> GetStates() => new List<State> {trueState, falseState};
    public State GetTrueState() => trueState;
    public State GetFalseState() => falseState;
}