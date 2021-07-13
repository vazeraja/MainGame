using ThunderNut.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputData/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions, GameInput.IMenuActions {
    
    // Gameplay
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction<float> FJumpEvent;
    public event UnityAction JumpEvent;
    public event UnityAction JumpCanceledEvent;
    public event UnityAction AttackEvent;
    public event UnityAction AttackCanceledEvent;
    public event UnityAction DashEvent;
    public event UnityAction DashCanceledEvent;
    public event UnityAction<Vector2> DashKeyboardEvent;

    // Interaction
    public event UnityAction InteractionStartedEvent;
    public event UnityAction InteractionCancelledEvent;

    // Developer Console
    public event UnityAction OpenDevConsole;

    // Dialogue
    public event UnityAction AdvanceDialogueEvent;

    // Menu Input
    public event UnityAction OpenMenuWindow;
    public event UnityAction TabRightButtonEvent;
    public event UnityAction TabLeftButtonEvent;
    public event UnityAction CloseMenuWindow;

    private GameInput GameInput { get; set; }

    private void OnEnable() {
        if (GameInput == null) {
            GameInput = new GameInput();
            GameInput.Gameplay.SetCallbacks(this);
            GameInput.Dialogues.SetCallbacks(this);
            GameInput.Menu.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    private void OnDisable() => DisableAllInput();

    #region Gameplay Actions

    public void OnJump(InputAction.CallbackContext context) {
        if (JumpEvent != null && context.phase == InputActionPhase.Started) {
            JumpEvent.Invoke();
            FJumpEvent?.Invoke(context.ReadValue<float>());
        }

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
        if (OpenMenuWindow != null && context.phase == InputActionPhase.Started)
            OpenMenuWindow?.Invoke();
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
        if (TabLeftButtonEvent != null && context.phase == InputActionPhase.Started) {
            TabLeftButtonEvent?.Invoke();
        }
    }

    public void OnTabRight(InputAction.CallbackContext context) {
        if (TabRightButtonEvent != null && context.phase == InputActionPhase.Started)
            TabRightButtonEvent?.Invoke();
    }

    public void OnCloseMenu(InputAction.CallbackContext context) {
        if (CloseMenuWindow != null && context.phase == InputActionPhase.Started)
            CloseMenuWindow?.Invoke();
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