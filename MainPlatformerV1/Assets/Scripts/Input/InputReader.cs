using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MainGame {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions {

        // Gameplay
        public event UnityAction<Vector2> moveEvent;
        public event UnityAction jumpEvent;
        public event UnityAction jumpCanceledEvent;
        public event UnityAction attackEvent;
        public event UnityAction attackCanceledEvent;
        public event UnityAction dashEvent;
        public event UnityAction dashCanceledEvent;
        public event UnityAction<Vector2> dashKeyboardEvent;

        public event UnityAction interactEvent; // Used to talk, pickup objects, interact with tools like the cooking cauldron
        public event UnityAction extraActionEvent; // Used to bring up the inventory
        public event UnityAction pauseEvent;
        public event UnityAction<Vector2, bool> cameraMoveEvent;
        public event UnityAction enableMouseControlCameraEvent;
        public event UnityAction disableMouseControlCameraEvent;

        // Dialogue
        public event UnityAction advanceDialogueEvent;
        public event UnityAction resetDialogueEvent;
        public event UnityAction onMoveSelectionEvent = delegate { };

        private GameInput gameInput;


        private void OnEnable() {
            if (gameInput == null) {
                gameInput = new GameInput();
                gameInput.Gameplay.SetCallbacks(this);
                gameInput.Dialogues.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable() {
            DisableAllInput();
        }

        public void OnAttack(InputAction.CallbackContext context) {
            if (attackEvent != null
                && context.phase == InputActionPhase.Performed)
                attackEvent.Invoke();

            if (attackCanceledEvent != null
                && context.phase == InputActionPhase.Canceled)
                attackCanceledEvent.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context) {
            if (dashEvent != null && context.phase == InputActionPhase.Performed)
                dashEvent.Invoke();

            if (dashCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                dashCanceledEvent.Invoke();
        }
        public void OnDashDirectionKeyboard(InputAction.CallbackContext context) {
            if (dashKeyboardEvent != null) {
                dashKeyboardEvent.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnExtraAction(InputAction.CallbackContext context) {
            if (extraActionEvent != null
                && context.phase == InputActionPhase.Performed)
                extraActionEvent.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context) {
            if (interactEvent != null
                && context.phase == InputActionPhase.Performed)
                interactEvent.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (jumpEvent != null
                && context.phase == InputActionPhase.Performed)
                jumpEvent.Invoke();

            if (jumpCanceledEvent != null
                && context.phase == InputActionPhase.Canceled)
                jumpCanceledEvent.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context) {
            if (moveEvent != null) {
                moveEvent.Invoke(context.ReadValue<Vector2>().normalized);
            }
        }

        public void OnPause(InputAction.CallbackContext context) {
            if (pauseEvent != null
                && context.phase == InputActionPhase.Performed)
                pauseEvent.Invoke();
        }

        public void OnRotateCamera(InputAction.CallbackContext context) {
            if (cameraMoveEvent != null) {
                cameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
            }
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
                enableMouseControlCameraEvent?.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                disableMouseControlCameraEvent?.Invoke();
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnMoveSelection(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
                onMoveSelectionEvent();
        }

        public void OnAdvanceDialogue(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
                advanceDialogueEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled)
                resetDialogueEvent?.Invoke();
        }

        public void EnableDialogueInput() {
            gameInput.Dialogues.Enable();
            gameInput.Gameplay.Disable();
        }

        public void EnableGameplayInput() {
            gameInput.Gameplay.Enable();
            gameInput.Dialogues.Disable();
        }

        public void DisableAllInput() {
            gameInput.Gameplay.Disable();
            gameInput.Dialogues.Disable();
        }
    }
}

