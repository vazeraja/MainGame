using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Player Base State")]
    public class PlayerStateSO : BaseState<Player, PlayerStateSO> {

        public string animBoolName;
        public PlayerStateSO(string stateName, State<Player>[] states, Transition<Player, PlayerStateSO>[] transitions,
            Action<Player> enterStateEvent, Action<Player> exitStateEvent, Action<Player> updateStateEvent, string animBoolName) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent) {
            this.animBoolName = animBoolName;
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

                player.TransitionToState(decisionSucceeded ? transitions[i].trueState : transitions[i].falseState);
                
            }
        }
    }
}
