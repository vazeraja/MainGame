using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MainGame {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions,
        GameInput.IMenuActions {
        // Gameplay
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction JumpEvent;
        public event UnityAction JumpCanceledEvent;
        public event UnityAction AttackEvent;
        public event UnityAction AttackCanceledEvent;
        public event UnityAction DashEvent;
        public event UnityAction DashCanceledEvent;
        public event UnityAction<Vector2> DashKeyboardEvent;

        // Menu
        public event UnityAction OpenMenuEvent;

        // Interaction
        public event UnityAction InteractionStartedEvent;
        public event UnityAction InteractionCancelledEvent;

        // Developer Console
        public event UnityAction OpenDevConsole;

        // Dialogue
        public event UnityAction AdvanceDialogueEvent;

        // Menu Input
        public event UnityAction TabRightButtonEventStarted;
        public event UnityAction TabRightButtonEventCancelled;
        public event UnityAction TabLeftButtonEventStarted;
        public event UnityAction TabLeftButtonEventCancelled;
        public event UnityAction CloseMenuStartedEvent;
        public event UnityAction CloseMenuCancelledEvent;

        private GameInput GameInput{ get; set; }

        private void OnEnable() {
            if (GameInput == null) {
                GameInput = new GameInput();
                GameInput.Gameplay.SetCallbacks(this);
                GameInput.Dialogues.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable() => DisableAllInput();

        #region Gameplay Actions

        public void OnJump(InputAction.CallbackContext context) {
            if (JumpEvent != null && context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();

            if (JumpCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context) {
            MoveEvent?.Invoke(context.ReadValue<Vector2>().normalized);
        }

        public void OnDash(InputAction.CallbackContext context) {
            if (DashEvent != null && context.phase == InputActionPhase.Performed)
                DashEvent.Invoke();

            if (DashCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                DashCanceledEvent.Invoke();
        }

        public void OnDashDirectionKeyboard(InputAction.CallbackContext context) {
            DashKeyboardEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context) {
            if (AttackEvent != null && context.phase == InputActionPhase.Performed)
                AttackEvent.Invoke();

            if (AttackCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                AttackCanceledEvent.Invoke();
        }

        public void OnOpenDevConsole(InputAction.CallbackContext context) {
            if (OpenDevConsole != null && context.phase == InputActionPhase.Performed)
                OpenDevConsole.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context) {
            if (InteractionStartedEvent != null && context.phase == InputActionPhase.Started)
                InteractionStartedEvent.Invoke();

            if (InteractionCancelledEvent != null && context.phase == InputActionPhase.Canceled)
                InteractionCancelledEvent.Invoke();
        }

        public void OnMenu(InputAction.CallbackContext context) {
            if (OpenMenuEvent != null && (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started))
                OpenMenuEvent?.Invoke();
        }

        #endregion

        #region Dialogue Actions

        public void OnAdvanceDialogue(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
                AdvanceDialogueEvent?.Invoke();
        }

        #endregion

        #region Menu Actions

        public void OnTabLeft(InputAction.CallbackContext context) {
            if (TabLeftButtonEventStarted != null && context.phase == InputActionPhase.Started)
                TabLeftButtonEventStarted?.Invoke();
            if (TabLeftButtonEventCancelled != null && context.phase == InputActionPhase.Canceled)
                TabLeftButtonEventCancelled?.Invoke();
        }

        public void OnTabRight(InputAction.CallbackContext context) {
            if (TabRightButtonEventStarted != null && context.phase == InputActionPhase.Started)
                TabRightButtonEventStarted?.Invoke();
            if (TabRightButtonEventCancelled != null && context.phase == InputActionPhase.Canceled)
                TabRightButtonEventCancelled?.Invoke();
        }

        public void OnCloseMenu(InputAction.CallbackContext context) {
            if (CloseMenuStartedEvent != null && context.phase == InputActionPhase.Started)
                CloseMenuStartedEvent?.Invoke();
            if (CloseMenuCancelledEvent != null && context.phase == InputActionPhase.Canceled)
                CloseMenuCancelledEvent?.Invoke();
        }

        #endregion

        public void EnableGameplayInput() {
            GameInput.Gameplay.Enable();
            GameInput.Dialogues.Disable();
            GameInput.Menu.Disable();
        }

        public void EnableDialogueInput() {
            GameInput.Dialogues.Enable();
            GameInput.Gameplay.Disable();
            GameInput.Menu.Disable();
        }

        public void EnableMenuInput() {
            GameInput.Menu.Enable();
            GameInput.Gameplay.Disable();
            GameInput.Dialogues.Disable();
        }

        public void DisableAllInput() {
            GameInput.Gameplay.Disable();
            GameInput.Dialogues.Disable();
            GameInput.Menu.Disable();
        }
    }
}