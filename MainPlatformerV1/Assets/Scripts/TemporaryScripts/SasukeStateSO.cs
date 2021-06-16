using System;
using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(menuName = "PluggableAI/SasukeBaseState")]
    public class SasukeStateSO : BaseState<SasukeController, SasukeStateSO> {
        
        public static event Action<SasukeStateSO> OnStateTransition;
        
        public SasukeStateSO(string stateName, State<SasukeController>[] states, Transition<SasukeController, SasukeStateSO>[] transitions, Action<SasukeController> enterStateEvent, Action<SasukeController> exitStateEvent, Action<SasukeController> updateStateEvent) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent) { }
        protected override void CheckTransitions(SasukeController sasuke) {
            for (var i = 0; i < transitions.Length; i++) {
                var decisionSucceeded = transitions[i].decision.Decide(sasuke);
                
                OnStateTransition?.Invoke(decisionSucceeded ? transitions[i].trueState : transitions[i].falseState);
            }
        }

        protected override void ResetAnimationFinished(SasukeController entity) {
        }
    }
}