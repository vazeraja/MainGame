using System;
using System.Collections.Generic;
using MainGame.Utils;
using TMPro;
using UnityEngine;
using static MainGame.PlayerStateSO;

namespace MainGame {
    public class Player : CustomPhysics {

        [Header("State Machine")] 
        public PlayerStateSO currentState;
        public PlayerStateSO remainState;

        [Header("Interaction System")]
        [SerializeField] private Optional<InteractionLogic> interactionLogic;
        
        [Header("GameEvents")] 
        [SerializeField] private PlayerEvent playerInitialized;
        // public CharacterStat Strength;

        #region Built-In Methods
        
        protected override void OnEnable() {
            base.OnEnable();
            OnStateTransition += TransitionToState;
        }

        protected override void OnDisable() {
            base.OnDisable();
            OnStateTransition -= TransitionToState;
        }

        protected override void Start() {
            base.Start();
            
            playerInitialized.Raise(this);
            
            currentState.Refresh();
            currentState.OnStateEnter(this);
        }

        protected override void Update() {
            base.Update();

            currentState.OnStateLogicUpdate(this);
            if(interactionLogic.Enabled) 
                interactionLogic.Value.UpdateInteractable(this, collider.bounds.center);
        }

        #endregion
        
        private void TransitionToState(PlayerStateSO nextState) {
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            if (currentState.animBoolName.Enabled) 
                Anim.SetBool(currentState.animBoolName.Value, false);
            currentState = nextState;
            if (currentState.animBoolName.Enabled) 
                Anim.SetBool(currentState.animBoolName.Value, true);
            currentState.OnStateEnter(this);
        }

        public void AnimationFinishTrigger() => currentState.AnimationFinishTrigger(); // Used as an Animation Event
        
        
    }
}