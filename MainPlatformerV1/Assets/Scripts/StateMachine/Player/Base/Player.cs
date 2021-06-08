using System;
using System.Collections.Generic;
using MainGame.Utils;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

namespace MainGame {
    public class Player : CustomPhysics {

        [Header("State Machine")] 
        public PlayerStateSO currentState;
        public PlayerStateSO remainState;
        public Action<PlayerStateSO> OnStateTransition;
        public Optional<TextMeshProUGUI> stateName;
        
        [Header("Interaction System")]
        [SerializeField] private InteractionLogic interactionLogic;
        
        [Header("GameEvents")] 
        [SerializeField] private PlayerEvent playerInitialized;
        
        // public CharacterStat Strength;

        #region Built-In Methods
        
        protected override void OnEnable() {
            base.OnEnable();
            OnStateTransition += TransitionToState;
            OnStateTransition += p => {
                if (stateName.Enabled) stateName.Value.text = currentState.stateName;
            };
        }

        private void OnDisable() {
            OnStateTransition -= TransitionToState;
            OnStateTransition -= p => {
                if (stateName.Enabled) stateName.Value.text = currentState.stateName;
            };
        }

        protected override void Start() {
            base.Start();
            
            playerInitialized.Raise(this);
            
            currentState.Refresh();
            currentState.OnStateEnter(this);
        }

        protected override void Update() {
            base.Update();

            currentState.OnStateUpdate(this);
            interactionLogic.UpdateInteractable(this, collider.bounds.center);
        }

        #endregion
        
        private void TransitionToState(PlayerStateSO nextState) {
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            if (currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, false);
            currentState = nextState;
            if (currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, true);
            currentState.OnStateEnter(this);
        }

        public void AnimationFinishTrigger() => currentState.AnimationFinishTrigger(); // Used as an Animation Event

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(collider.bounds.center, 0.5f);
        }
        
    }
}