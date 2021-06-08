using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/Player Base State")]
    public class PlayerStateSO : BaseState<Player, PlayerStateSO> {
        public static event Action<PlayerStateSO> OnStateTransition;
        
        public PlayerData playerData;
        
        [Space] 
        public Optional<string> animBoolName;
        
        public PlayerStateSO(string stateName, State<Player>[] states, Transition<Player, PlayerStateSO>[] transitions,
            Action<Player> enterStateEvent, Action<Player> exitStateEvent, Action<Player> updateStateEvent, string animBoolName, PlayerData playerData) : base(stateName, states, transitions, enterStateEvent, exitStateEvent, updateStateEvent){
            this.playerData = playerData;
            this.animBoolName.Value = animBoolName;
        }

        public void Refresh() => playerData.Reset();
        
        protected override void CheckTransitions(Player player){
            for (var i = 0; i < transitions.Length; i++) {
                var decisionSucceeded = transitions[i].decision.Decide(player);
                
                OnStateTransition?.Invoke(decisionSucceeded ? transitions[i].trueState : transitions[i].falseState);
            }
        }
        protected override void ResetAnimationFinished(Player player) => playerData.isAnimationFinished = false;
        
        // Animation Events
        public void AnimationFinishTrigger() => playerData.isAnimationFinished = true;
    }
}
