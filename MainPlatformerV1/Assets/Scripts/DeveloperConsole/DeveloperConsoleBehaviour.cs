using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MainGame.DeveloperConsole {
    public class DeveloperConsoleBehaviour : MonoBehaviour {
        
        // Consider making it DontDestroyOnLoad when we have multiple scenes
        // ----------------------------------------------------------------------------
        // Lazy loading: Doesnt create the singleton instance until you request for one
        private static DeveloperConsoleBehaviour instance;
        public static DeveloperConsoleBehaviour Instance => instance ??= CreateNewInstance();
        private static DeveloperConsoleBehaviour CreateNewInstance(){
            var gameObject = new GameObject("DeveloperConsoleBehaviour");
            return gameObject.AddComponent<DeveloperConsoleBehaviour>();
        }

        private DeveloperConsole developerConsole;
        private DeveloperConsole DeveloperConsole {
            get {
                if (developerConsole != null) { return developerConsole; }
                return developerConsole = new DeveloperConsole(prefix, commands);
            }
        }

        [SerializeField] private string prefix = string.Empty;
        [SerializeField] private InputReader playerInput = null;
        [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

        [Header("UI")]
        [SerializeField] private GameObject uiCanvas = null;
        [SerializeField] private TMP_InputField inputField = null;

        private void Awake(){
            inputField.onEndEdit.AddListener( (x) => ProcessCommand(inputField.text));
        }

        private float pausedTimeScale;
        
        public void Toggle(InputAction.CallbackContext context){
            if (!context.action.triggered) { return; }

            if (uiCanvas.activeSelf) {
                Time.timeScale = pausedTimeScale;
                uiCanvas.SetActive(false);
                playerInput.GameInput.Enable();
            }
            else {
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                playerInput.GameInput.Disable();
                inputField.ActivateInputField();
            }
        }
        public void ProcessCommand(string inputValue){
            DeveloperConsole.ProcessCommand(inputValue);
            uiCanvas.SetActive(false);
            inputField.text = string.Empty;
        }

    }
}
