﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void StateMachineListener();

public class StateMachine : MonoBehaviour, IStateMachine {
    public List<State> states = new List<State>();
    public List<Decision> decisions = new List<Decision>();

    public event StateMachineListener OnStateTransition;

    [field: SerializeField] public State CurrentState { get; private set; }
    [field: SerializeField] public State RemainState { get; private set; }

    public void OnEnable() {
        states.ForEach(state => state.CheckTransitions += TransitionToState);
    }

    public void OnDisable() {
        states.ForEach(state => state.CheckTransitions -= TransitionToState);
    }

    public void Update() {
        CurrentState.Update();
        CurrentState.CheckStateTransitions();
    }

    public void FixedUpdate() {
        CurrentState.FixedUpdate();
    }

    public void TransitionToState(State nextState) {
        if (nextState == RemainState)
            return;

        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
        OnStateTransition?.Invoke();
    }

    public void Bind(SasukeController player) {
        states.ForEach(state => state.player = player);
        decisions.ForEach(decision => decision.Player = player);
        states.ForEach(state => state.stateMachine = this);
    }
}