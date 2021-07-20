using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputData", menuName = "InputData/PlayerInputData")]
public class PlayerInputData : ScriptableObject {
    [SerializeField] private InputReader inputReader = null;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private bool attackInput = false;
    [SerializeField] private bool dashInput = false;

    // Properties
    public Vector2 MovementInput {
        get => movementInput;
        set => movementInput = value;
    }

    public bool AttackInput {
        get => attackInput;
        set => attackInput = value;
    }

    public bool DashInput {
        get => dashInput;
        set => dashInput = value;
    }


    public void OnEnable() {
        try {
            inputReader.MoveEvent += OnMove;
            inputReader.DashEvent += OnDashInitiated;
            inputReader.DashCanceledEvent += OnDashCancelled;
            inputReader.AttackEvent += OnAttackInitiated;
            inputReader.AttackCanceledEvent += OnAttackCanceled;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void OnDisable() {
        try {
            inputReader.MoveEvent -= OnMove;
            inputReader.DashEvent -= OnDashInitiated;
            inputReader.DashCanceledEvent -= OnDashCancelled;
            inputReader.AttackEvent -= OnAttackInitiated;
            inputReader.AttackCanceledEvent -= OnAttackCanceled;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }


    private void OnMove(Vector2 input) => MovementInput = input;
    private void OnDashInitiated() => DashInput = true;
    private void OnDashCancelled() => DashInput = false;
    private void OnAttackInitiated() => AttackInput = true;
    private void OnAttackCanceled() => AttackInput = false;

    public void EnableGameplayInput() => inputReader.EnableGameplayInput();

    public void Reset() {
        movementInput = Vector2.zero;
        attackInput = false;
        dashInput = false;
    }
}