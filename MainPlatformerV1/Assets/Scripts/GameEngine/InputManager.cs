using UnityEngine;


public class InputManager : MonoBehaviour {
    
    public string description;
    
    [Space]
    [SerializeField] private InputProvider inputProvider;

    [Space]
    [SerializeField] private PlayerInputData playerInputData;
    [SerializeField] private InteractionInputData interactionInputData;

    private void Start() {
        playerInputData.Reset();
        interactionInputData.Reset();
    }

    public InputProvider GetInputReader() => inputProvider;
}