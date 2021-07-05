using System;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

public class BuilderTester : MonoBehaviour {
    public RuntimeStateMachine stateMachine;

    private void Awake() {
        stateMachine = Builder.RuntimeStateMachine
            .WithState(ScriptableObject.CreateInstance<MovementState>(), out var movementState)
            .WithState(ScriptableObject.CreateInstance<DashState>(), out var dashState)
            .WithState(ScriptableObject.CreateInstance<SasukeHitState>(), out var hitState)
            .WithState(ScriptableObject.CreateInstance<RemainState>(), out var remainState)
            .WithTransition(ScriptableObject.CreateInstance<Transition>(),
                ScriptableObject.CreateInstance<EnterMovementStateDecision>(), movementState, remainState,
                new[] {dashState, hitState})
            .WithTransition(ScriptableObject.CreateInstance<Transition>(),
                ScriptableObject.CreateInstance<EnterDashStateDecision>(), dashState, remainState, movementState)
            .WithTransition(ScriptableObject.CreateInstance<Transition>(),
                ScriptableObject.CreateInstance<EnterHitStateDecision>(), hitState, remainState,
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