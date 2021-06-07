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
        
        [Header("GameEvents")] 
        [SerializeField] private PlayerEvent playerInitialized;
        
        private InteractionHandler interactionHandler;

        // public CharacterStat Strength;

        #region Built-In Methods

        private void Awake() {
            interactionHandler = gameObject.AddComponent<InteractionHandler>();
        }

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
            interactionHandler.UpdateInteractable(Collider2D);
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
            Gizmos.DrawWireSphere(Collider2D.bounds.center, 0.5f);
        }

        private class InteractionHandler : MonoBehaviour {

            private PlayerData playerData;
            private InteractionInputData interactionInputData;
            private InteractionData interactionData;
            private void Start() {
                playerData = Resources.Load<PlayerData>("StateMachine/Player/PlayerData");
                interactionInputData = Resources.Load<InteractionInputData>("Input/InteractionInputData");
                interactionData = Resources.Load<InteractionData>("InteractionSystem/InteractionData");
            }

            public void UpdateInteractable(Collider2D bounds) {
                CheckForInteractable(bounds);
                CheckForInteractableInput();
            }
            private void CheckForInteractable(Collider2D bounds) {
                var hitSomething = Helper.Raycast(bounds.bounds.center,
                    transform.right, playerData.rayDistance, playerData.interactableLayer, out var ray);
                if (hitSomething) {
                    var interactable = ray.transform.GetComponent<InteractableBase>();
                    if (interactable != null) {
                        if (interactionData.IsEmpty()) {
                            interactionData.Interactable = interactable;
                        }
                        else {
                            if (!interactionData.IsSameInteractable(interactable)) {
                                interactionData.Interactable = interactable;
                            }
                        }
                    }
                }
                else {
                    interactionData.ResetData();
                }

                Debug.DrawRay(bounds.bounds.center, transform.right * playerData.rayDistance,
                    hitSomething ? Color.green : Color.red);
            }

            private void CheckForInteractableInput() {
                if (interactionData.IsEmpty() || !interactionInputData.isInteracting ||
                    !interactionData.Interactable.IsInteractable) return;

                if (interactionData.Interactable.HoldInteract) {
                    interactionInputData.holdTimer += Time.deltaTime;
                    if (!(interactionInputData.holdTimer >= interactionData.Interactable.HoldDuration)) return;
                    interactionData.Interact();
                    interactionInputData.isInteracting = false;
                }
                else {
                    interactionData.Interact();
                    interactionInputData.isInteracting = false;
                }
            }
            
        }
    }
}