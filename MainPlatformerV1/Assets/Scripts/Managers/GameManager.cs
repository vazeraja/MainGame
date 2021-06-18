using System;
using System.Collections.Generic;
using MainGame.DeveloperConsole;
using MainGame.Utils;
using UnityEngine;
using static MainGame.DeveloperConsole.DeveloperConsoleBehaviour;

namespace MainGame {
    public class GameManager : MonoBehaviour {
        
        [Header("Game Events & Listeners")]
        [SerializeField] private GameEventListenerSO<Player, PlayerEvent, UnityPlayerEvent> playerListener;
        
        private InputHandler inputHandler;
        
        private Player activePlayer;
        private readonly Vector3 spawnPoint = new Vector3(-9f, 1f, 0f);

        private void Awake() {
            inputHandler = gameObject.AddComponent<InputHandler>();
            playerListener.UnityEvent.AddListener(RegisterPlayer);
        }

        // --- Event Listeners --- 
        public void RegisterPlayer(Player player){
            activePlayer = player;
            Helper.CustomLog("GameManager: Player Initialized", LogColor.Green);
        }
        
    }

}
