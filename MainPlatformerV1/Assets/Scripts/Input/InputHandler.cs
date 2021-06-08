using System;
using UnityEngine;

namespace MainGame {
    public class InputHandler : MonoBehaviour {
        [Header("Input")] 
        [SerializeField] private InputReader inputReader = default;
        
        [Space]
        [SerializeField] private InteractionInputData interactionInputData = default;
        [SerializeField] private PlayerInputData playerInputData = default;
        [SerializeField] private MenuInputData menuInputData = default;
        
        private void OnEnable() {
            inputReader.EnableGameplayInput();
            playerInputData.RegisterEvents();
            interactionInputData.RegisterEvents();
            menuInputData.RegisterEvents();
        }
        private void OnDisable() {
            playerInputData.UnregisterEvents();
            interactionInputData.UnregisterEvents();
            menuInputData.UnregisterEvents();
        }

        private void Start() {
            playerInputData.Reset();
            interactionInputData.Reset();
            menuInputData.Reset();
        }
    }
}