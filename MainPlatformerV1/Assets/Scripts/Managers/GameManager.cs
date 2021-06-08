using System;
using System.Collections.Generic;
using Cinemachine;
using MainGame.DeveloperConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame {
    public class GameManager : MonoBehaviour {
        
        [Header("Input")]
        [SerializeField] private InputReader inputReader = null;

        [Header("Developer Console")]
        [SerializeField] private List<ConsoleCommand> commands = new List<ConsoleCommand>();
        
        [SerializeField] private GameEventListenerSO<Player, PlayerEvent, UnityPlayerEvent> playerListener;
        
        [SerializeField] private GameObject menuWindow;
        
        private Player activePlayer;

        private readonly Vector3 spawnPoint = new Vector3(-9f, 1f, 0f);
        private void Awake(){
            playerListener.UnityEvent.AddListener(RegisterPlayer);
        }
        private void OnEnable(){
            inputReader.OpenDevConsole += OpenDevConsole;
            inputReader.OpenMenuEvent += OpenMenuWindow;
        }
        private void OnDisable(){
            inputReader.OpenDevConsole -= OpenDevConsole;
            inputReader.OpenMenuEvent -= OpenMenuWindow;
        }

        private void Update() {
            //OpenMenuWindow();
        }

        public void RegisterPlayer(Player player){
            activePlayer = player;
            Debug.Log("<b><color=white>GameManager: Player Initialized </color></b>");
        }

        
        // --- Event Listeners --- 
        private void OpenDevConsole() => DeveloperConsoleBehaviour.GetDevConsole(commands, inputReader, "/");

        private void OpenMenuWindow() { menuWindow.SetActive(true); inputReader.EnableMenuInput(); }
    }

}
