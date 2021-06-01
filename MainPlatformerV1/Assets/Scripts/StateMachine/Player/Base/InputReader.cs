using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MainGame {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions {

        // Gameplay
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction JumpEvent;
        public event UnityAction JumpCanceledEvent;
        public event UnityAction AttackEvent;
        public event UnityAction AttackCanceledEvent;
        public event UnityAction DashEvent;
        public event UnityAction DashCanceledEvent;
        public event UnityAction<Vector2> DashKeyboardEvent;

        // Dialogue
        public event UnityAction AdvanceDialogueEvent;
        public event UnityAction ResetDialogueEvent;

        // Developer Console
        public event UnityAction OpenDevConsole;

        public GameInput GameInput { get; private set; }

        private void OnEnable(){
            if (GameInput == null) {
                GameInput = new GameInput();
                GameInput.Gameplay.SetCallbacks(this);
                GameInput.Dialogues.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable() => DisableAllInput();

        #region Gameplay Actions
        public void OnJump(InputAction.CallbackContext context){
            if (JumpEvent != null && context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();

            if (JumpCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context){
            MoveEvent?.Invoke(context.ReadValue<Vector2>().normalized);
        }

        public void OnDash(InputAction.CallbackContext context){
            if (DashEvent != null && context.phase == InputActionPhase.Performed)
                DashEvent.Invoke();

            if (DashCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                DashCanceledEvent.Invoke();
        }

        public void OnDashDirectionKeyboard(InputAction.CallbackContext context){
            DashKeyboardEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context){
            if (AttackEvent != null && context.phase == InputActionPhase.Performed)
                AttackEvent.Invoke();

            if (AttackCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                AttackCanceledEvent.Invoke();
        }
        #endregion
        
        // Dev Console
        public void OnOpenDevConsole(InputAction.CallbackContext context){
            if (OpenDevConsole != null && context.phase == InputActionPhase.Performed)
                OpenDevConsole.Invoke();
        }

        #region Dialogue Actions
        public void OnAdvanceDialogue(InputAction.CallbackContext context){
            if (context.phase == InputActionPhase.Performed)
                AdvanceDialogueEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled)
                ResetDialogueEvent?.Invoke();
        }
        #endregion


        public void EnableGameplayInput(){
            GameInput.Gameplay.Enable();
            GameInput.Dialogues.Disable();
        }
        public void EnableDialogueInput(){
            GameInput.Dialogues.Enable();
            GameInput.Gameplay.Disable();
        }

        public void DisableAllInput(){
            GameInput.Gameplay.Disable();
            GameInput.Dialogues.Disable();
        }

    }
}
