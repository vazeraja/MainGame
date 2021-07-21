using UnityEngine;


public class InputManager : MonoBehaviour {
    
    public string description;
    
    [Space]
    [SerializeField] private InputProvider inputProvider;

    [Space]
    [SerializeField] private InteractionInputData interactionInputData;

    private void Start() {
        interactionInputData.Reset();
    }

    public InputProvider GetInputReader() => inputProvider;
}