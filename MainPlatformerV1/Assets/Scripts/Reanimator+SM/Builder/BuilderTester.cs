using System;
using System.Collections.Generic;
using ThunderNut.SceneManagement;
using UnityEngine;

public class BuilderTester : MonoBehaviour {
    
    private RuntimeStateMachine stateMachine;

    public SceneHandle sceneHandle;

    private void Awake() {
        stateMachine = Builder.RuntimeStateMachine
            .WithState(new MovementState(), out var movementState)
            .WithState(new DashState(), out var dashState)
            .WithState(new HitState(), out var hitState)
            .WithState(new RemainState(), out var remainState)
            .WithTransition(new Transition(), new EnterMovementStateDecision(), movementState, remainState,
                new[] {dashState, hitState})
            .WithTransition(new Transition(), new EnterDashStateDecision(), dashState, remainState, 
                new[] {movementState})
            .WithTransition(new Transition(), new EnterHitStateDecision(), hitState, remainState,
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