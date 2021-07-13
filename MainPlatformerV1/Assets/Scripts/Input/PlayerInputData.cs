using System;
using ThunderNut.Extensions;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputData", menuName = "InputData/PlayerInputData")]
public class PlayerInputData : ScriptableObject {
    [SerializeField] private InputReader inputReader = null;

    [Header("Game Input")] [Space] [SerializeField]
    private Vector2 movementInput;

    [SerializeField] private bool jumpInput;
    [SerializeField] private bool attackInput = false;
    [SerializeField] private bool dashInput = false;
    [SerializeField] private Vector2 dashKeyboardInput;

    // Properties
    public Vector2 MovementInput {
        get => movementInput;
        set => movementInput = value;
    }

    public bool JumpInput {
        get => jumpInput;
        set => jumpInput = value;
    }

    public bool AttackInput {
        get => attackInput;
        set => attackInput = value;
    }

    public bool DashInput {
        get => dashInput;
        set => dashInput = value;
    }

    public Vector2 DashKeyboardInput {
        get => dashKeyboardInput;
        set => dashKeyboardInput = value;
    }


    public void RegisterEvents() {
        try {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJumpInitiated;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
            inputReader.DashEvent += OnDashInitiated;
            inputReader.DashCanceledEvent += OnDashCancelled;
            inputReader.DashKeyboardEvent += OnDashKeyboard;
            inputReader.AttackEvent += OnAttackInitiated;
            inputReader.AttackCanceledEvent += OnAttackCanceled;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void UnregisterEvents() {
        try {
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJumpInitiated;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;
            inputReader.DashEvent -= OnDashInitiated;
            inputReader.DashCanceledEvent -= OnDashCancelled;
            inputReader.DashKeyboardEvent -= OnDashKeyboard;
            inputReader.AttackEvent -= OnAttackInitiated;
            inputReader.AttackCanceledEvent -= OnAttackCanceled;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }


    private void OnMove(Vector2 input) => MovementInput = input;
    private void OnJumpInitiated() => JumpInput = true;
    private void OnJumpCanceled() => JumpInput = false;
    private void OnDashInitiated() => DashInput = true;
    private void OnDashCancelled() => DashInput = false;
    private void OnDashKeyboard(Vector2 input) => DashKeyboardInput = input;
    private void OnAttackInitiated() => AttackInput = true;
    private void OnAttackCanceled() => AttackInput = false;

    public void EnableGameplayInput() => inputReader.EnableGameplayInput();

    public void Reset() {
        movementInput = Vector2.zero;
        jumpInput = false;
        attackInput = false;
        dashInput = false;
        dashKeyboardInput = Vector2.zero;
    }
}