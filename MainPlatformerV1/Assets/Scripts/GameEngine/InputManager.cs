using UnityEngine;


public class InputManager : MonoBehaviour {
    [Space, SerializeField] private InputReader inputReader;

    [Space, SerializeField] private PlayerInputData playerInputData;
    [SerializeField] private InteractionInputData interactionInputData;

    private void OnEnable() {
        inputReader.EnableGameplayInput();
        playerInputData.RegisterEvents();
        interactionInputData.RegisterEvents();
    }

    private void OnDisable() {
        playerInputData.UnregisterEvents();
        interactionInputData.UnregisterEvents();
    }

    private void Start() {
        playerInputData.Reset();
        interactionInputData.Reset();
    }
}