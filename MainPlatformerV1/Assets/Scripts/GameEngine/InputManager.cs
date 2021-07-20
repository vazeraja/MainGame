using UnityEngine;


public class InputManager : MonoBehaviour {
    
    public string description;
    
    [Space]
    [SerializeField] private InputReader inputReader;

    [Space]
    [SerializeField] private PlayerInputData playerInputData;
    [SerializeField] private InteractionInputData interactionInputData;

    private void Start() {
        playerInputData.Reset();
        interactionInputData.Reset();
    }

    public InputReader GetInputReader() => inputReader;
}