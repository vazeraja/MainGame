using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Base State")]
    public class State_SO : ScriptableObject {
        public string stateName;

        [Space(10)]
        public State[] states;
        public Transition[] transitions;

        public event Action<Player> enterStateEvent;
        public event Action<Player> updateStateEvent;
        public event Action<Player> exitStateEvent;

        private void OnEnable() {
            foreach (State state in states) {
                enterStateEvent += state.OnEnter;
                updateStateEvent += state.LogicUpdate;
                exitStateEvent += state.OnExit;
            }

            updateStateEvent += CheckTransitions;
            exitStateEvent += ResetAnimationFinished;
        }
        private void OnDisable() {
            foreach (State state in states) {
                enterStateEvent -= state.OnEnter;
                updateStateEvent -= state.LogicUpdate;
                exitStateEvent -= state.OnExit;
            }

            updateStateEvent -= CheckTransitions;
            exitStateEvent -= ResetAnimationFinished;
        }
        public void OnStateEnter(Player player) => enterStateEvent?.Invoke(player);
        public void OnLogicUpdate(Player player) => updateStateEvent?.Invoke(player);
        public void OnStateExit(Player player) => exitStateEvent?.Invoke(player);

        public void ResetAnimationFinished(Player player) => player.isAnimationFinished = false;


        public void CheckTransitions(Player player) {
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
