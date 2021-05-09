using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public class Enemy : CustomPhysics<EnemyState_SO> {

        public EnemyState_SO currentState;
        public EnemyState_SO remainState;
        [HideInInspector] public bool isAnimationFinished;

        public override void TransitionToState(EnemyState_SO nextState) {
            if (nextState != remainState) {
                currentState.OnStateExit(this);
                currentState = nextState;
                currentState.OnStateEnter(this);
            }
        }

    }
}
