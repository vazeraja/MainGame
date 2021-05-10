using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public class Enemy : CustomPhysics, IStateMachine<EnemyStateSO> {

        public EnemyStateSO currentState;
        public EnemyStateSO remainState;
        [HideInInspector] public bool isAnimationFinished;

        public void TransitionToState(EnemyStateSO nextState) {
            if (nextState != remainState) {
                currentState.OnStateExit(this);
                currentState = nextState;
                currentState.OnStateEnter(this);
            }
        }

    }
}
