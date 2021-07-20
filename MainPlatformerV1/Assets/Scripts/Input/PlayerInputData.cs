using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputData", menuName = "InputData/PlayerInputData")]
public class PlayerInputData : ScriptableObject {
    [SerializeField] private InputProvider inputProvider = null;

    [SerializeField] private Vector2 movementInput;
    // Properties
    public Vector2 MovementInput {
        get => movementInput;
        set => movementInput = value;
    }
    public void OnEnable() {
        try {
            inputProvider.MoveEvent += OnMove;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void OnDisable() {
        try {
            inputProvider.MoveEvent -= OnMove;
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }


    private void OnMove(Vector2 input) => MovementInput = input;


    public void Reset() {
        movementInput = Vector2.zero;
    }
}