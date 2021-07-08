using System;
using System.Collections.Generic;
using MainGame;
using UnityEngine;
using UnityEngine.LowLevel;

public class BuilderTester : MonoBehaviour {
    public RuntimeStateMachine stateMachine;

    private void Awake() {
        stateMachine = Builder.RuntimeStateMachine
            .WithState(ScriptableObject.CreateInstance<MovementState>(), out var movementState)
            .WithState(ScriptableObject.CreateInstance<DashState>(), out var dashState)
            .WithState(ScriptableObject.CreateInstance<HitState>(), out var hitState)
            .WithState(ScriptableObject.CreateInstance<RemainState>(), out var remainState)
            .WithTransition(new Transition(), ScriptableObject.CreateInstance<EnterMovementStateDecision>(), movementState, remainState,
                new[] {dashState, hitState})
            .WithTransition(new Transition(), ScriptableObject.CreateInstance<EnterDashStateDecision>(), dashState, remainState, 
                new[] {movementState})
            .WithTransition(new Transition(), ScriptableObject.CreateInstance<EnterHitStateDecision>(), hitState, remainState,
                new[] {movementState, dashState})
            .SetCurrentState(movementState)
            .SetRemainState(remainState);
    }

    private void Start() {
        stateMachine.Bind(GetComponent<SasukeController>());
    }

    private void Update() {
        stateMachine.Update();
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }

    private void OnDisable() {
        stateMachine.DisableTransitions();
    }
}