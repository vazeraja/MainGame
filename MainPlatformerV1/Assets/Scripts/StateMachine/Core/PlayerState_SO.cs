using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Player Base State")]
    public class PlayerState_SO : BaseState<Player, PlayerState_SO> {
        public PlayerState_SO(string stateName, State<Player>[] states, Transition<Player, PlayerState_SO>[] transitions, Action<Player> enterStateEvent,
            Action<Player> exitStateEvent, Action<Player> updateStateEvent) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent) {
        }

        protected override void OnEnable() {
            base.OnEnable();

        }
        protected override void OnDisable() {
            base.OnDisable();
        }

        protected override void ResetAnimationFinished(Player player) => player.isAnimationFinished = false;

        protected override void CheckTransitions(Player player) {
            for (int i = 0; i < transitions.Length; i++) {
                bool decisionSucceeded = transitions[i].decision.Decide(player);

                if (decisionSucceeded) {
                    player.TransitionToState(transitions[i].trueState);
                } else {
                    player.TransitionToState(transitions[i].falseState);
                }
            }
        }

    }
}
