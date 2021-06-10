using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.DeveloperConsole;
using UnityEngine;

namespace MainGame {
    public class UIManager : MonoBehaviour {
        
        [Header("Input")]
        [SerializeField] private InputReader inputReader = null;
        
        [Space, Header("Developer Console")]
        [SerializeField] private List<ConsoleCommand> commands = new List<ConsoleCommand>();
        
        [Header("Menu")]
        public GameObject menu;
        public TabGroup tabGroup;

        private void Awake() {
            menu.SetActive(false);
        }

        private void OnEnable() {
            inputReader.OpenDevConsole += OpenDevConsole;
            
            inputReader.OpenMenuWindow += OpenMenuWindow;
            inputReader.TabRightButtonEvent += OnTabRightButton;
            inputReader.TabLeftButtonEvent += OnTabLeftButton;
            inputReader.CloseMenuWindow += CloseMenuWindow;
        }
        private void OnDisable() {
            inputReader.OpenDevConsole -= OpenDevConsole;
            
            inputReader.OpenMenuWindow -= OpenMenuWindow;
            inputReader.TabRightButtonEvent -= OnTabRightButton;
            inputReader.TabLeftButtonEvent -= OnTabLeftButton;
            inputReader.CloseMenuWindow -= CloseMenuWindow;
        }
        
        // --- Event Listeners --- // 
        private void OpenDevConsole() => DeveloperConsoleBehaviour.GetDevConsole(commands, inputReader, "/");
        
        private void OpenMenuWindow() { menu.SetActive(true); inputReader.EnableMenuInput(); }
        private void OnTabRightButton() => tabGroup.OnTabRightButton();
        private void OnTabLeftButton() => tabGroup.OnTabLeftButton();
        private void CloseMenuWindow() { menu.SetActive(false); inputReader.EnableGameplayInput(); }
    }
}