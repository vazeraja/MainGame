using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Player Base State")]
    public class PlayerStateSO : BaseState<MainPlayer, PlayerStateSO> {

        public string animBoolName;
        public PlayerStateSO(string stateName, State<MainPlayer>[] states, Transition<MainPlayer, PlayerStateSO>[] transitions,
            Action<MainPlayer> enterStateEvent, Action<MainPlayer> exitStateEvent, Action<MainPlayer> updateStateEvent, string animBoolName) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent){
            this.animBoolName = animBoolName;
        }

        protected override void OnEnable(){
            base.OnEnable();
        }

        protected override void OnDisable(){
            base.OnDisable();
        }

        protected override void ResetAnimationFinished(MainPlayer mainPlayer){
            mainPlayer.isAnimationFinished = false;
        }

        protected override void CheckTransitions(MainPlayer mainPlayer){
            for (int i = 0; i < transitions.Length; i++) {
                bool decisionSucceeded = transitions[i].decision.Decide(mainPlayer);

                mainPlayer.TransitionToState(decisionSucceeded ? transitions[i].trueState : transitions[i].falseState);

            }
        }
    }
}
