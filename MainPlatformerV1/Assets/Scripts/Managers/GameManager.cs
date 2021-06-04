using System;
using System.Collections.Generic;
using Cinemachine;
using MainGame.DeveloperConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame {
    public class GameManager : MonoBehaviour {

        [SerializeField] private InputReader inputReader = null;

        [Header("Developer Console")]
        [SerializeField] private List<ConsoleCommand> commands = new List<ConsoleCommand>();

        public GameEventListenerSO<MainPlayer, PlayerEvent, UnityPlayerEvent> playerListener;
        private MainPlayer activePlayer;

        private readonly Vector3 spawnPoint = new Vector3(-9f, 1f, 0f);
        private void Awake(){
            playerListener.UnityEvent.AddListener(RegisterPlayer);
        }
        private void OnEnable(){
            inputReader.OpenDevConsole += OpenDevConsole;
        }
        private void OnDisable(){
            inputReader.OpenDevConsole -= OpenDevConsole;
        }

        public void RegisterPlayer(MainPlayer mainPlayer){
            Debug.Log("<b><color=white>GameManager: Player Initialized </color></b>");
            activePlayer = mainPlayer;
        }
        
        public void SpawnPlayer(Scene scene, LoadSceneMode mode){
            var localToWorldMatrix = transform.localToWorldMatrix;

            var playerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), (localToWorldMatrix * spawnPoint), Quaternion.identity);
            playerPrefab.name = "Player";

            var cam = Instantiate(Resources.Load<GameObject>("Prefabs/CinemachineVCam"), (localToWorldMatrix * new Vector4(0, 0, 0, 0)), Quaternion.identity);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = playerPrefab.transform;
        }
        public void DestroyPlayer() => Destroy(activePlayer.gameObject);

        // --- Event Listeners --- 
        private void OpenDevConsole() => DeveloperConsoleBehaviour.GetDevConsole(commands, inputReader, "/");

    }

}
