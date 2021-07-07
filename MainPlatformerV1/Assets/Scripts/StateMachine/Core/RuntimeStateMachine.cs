#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RuntimeStateMachine : ScriptableObject {
    
    public List<State> states = new List<State>();
    public List<Decision> decisions = new List<Decision>();

    public event StateMachineListener OnStateTransition;

    [Header("State Machine")] 
    public State currentState;
    public State remainState;

    public void Update() {
        currentState.Update();
        currentState.CheckStateTransitions();
    }

    public void FixedUpdate() {
        currentState.FixedUpdate();
    }
    public void EnableTransitions() {
        states.ForEach(state => state.CheckTransitions -= TransitionToState);
        states.ForEach(state => state.CheckTransitions += TransitionToState);
    }

    public void DisableTransitions() {
        states.ForEach(state => state.CheckTransitions -= TransitionToState);
    }
    
    private void TransitionToState(State nextState) {
        if (nextState == remainState)
            return;

        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
        OnStateTransition?.Invoke();
    }
    
    #region Editor
    #if UNITY_EDITOR
    public State CreateState(Type type) {
        State state = CreateInstance(type) as State;
        
        // ReSharper disable once PossibleNullReferenceException
        state.name = type.Name;
        state.guid = GUID.Generate().ToString();
        states.Add(state);
        
        AssetDatabase.AddObjectToAsset(state, this);
        AssetDatabase.SaveAssets();
        
        return state;
    }
    public void DeleteState(State state) {
        states.Remove(state);
        
        AssetDatabase.RemoveObjectFromAsset(state);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(State parent, State child) {
        ActionState state = parent as ActionState;
        if (state) state.children.Add(child);
    }
    public void RemoveChild(State parent, State child) {
        ActionState state = parent as ActionState;
        if (state) state.children.Remove(child);
    }
    public List<State> GetChildren(State parent) {
        List<State> children = new List<State>();
        
        ActionState state = parent as ActionState;
        return state ? state.children : children;
    }
    
    #endif
    #endregion

    public void Bind(SasukeController player) {
        states.ForEach(state => state.player = player);
        decisions.ForEach(decision => decision.player = player);
    }
    
}