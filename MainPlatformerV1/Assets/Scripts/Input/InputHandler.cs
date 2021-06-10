using System;
using UnityEngine;

namespace MainGame {
    public class InputHandler : MonoBehaviour {
        [Header("Input")] 
        [SerializeField] private InputReader inputReader = default;
        
        [Space]
        [SerializeField] private InteractionInputData interactionInputData = default;
        [SerializeField] private PlayerInputData playerInputData = default;

        private void Awake() {
            inputReader = Resources.Load<InputReader>("Input/InputReader");
            interactionInputData = Resources.Load<InteractionInputData>("Input/InteractionInputData");
            playerInputData = Resources.Load<PlayerInputData>("Input/PlayerInputData");
        }

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
}