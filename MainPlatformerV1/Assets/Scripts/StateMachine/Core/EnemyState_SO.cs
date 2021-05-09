using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(menuName = "PluggableAI/Enemy Base State")]
    public class EnemyState_SO : BaseState<Enemy, EnemyState_SO> {
        public EnemyState_SO(string stateName, State<Enemy>[] states, Transition<Enemy, EnemyState_SO>[] transitions, Action<Enemy> enterStateEvent, Action<Enemy> exitStateEvent, Action<Enemy> updateStateEvent) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent) {
        }

        protected override void OnEnable() {
            base.OnEnable();

        }
        protected override void OnDisable() {
            base.OnDisable();
        }

        protected override void CheckTransitions(Enemy enemy) {
            for (int i = 0; i < transitions.Length; i++) {
                bool decisionSucceeded = transitions[i].decision.Decide(enemy);

                if (decisionSucceeded) {
                    enemy.TransitionToState(transitions[i].trueState);
                } else {
                    enemy.TransitionToState(transitions[i].falseState);
                }
            }
        }

        protected override void ResetAnimationFinished(Enemy enemy) => enemy.isAnimationFinished = false;
    }
}
