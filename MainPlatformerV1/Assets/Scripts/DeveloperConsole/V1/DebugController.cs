using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MainGame {
    public class DebugController : MonoBehaviour {

        [SerializeField] private InputActionMap DevConsoleInputMap = new InputActionMap();
        private ReadOnlyArray<InputAction> Actions => DevConsoleInputMap.actions;

        private bool showConsole;
        private string input;

        public static DebugCommand SPAWN_PLAYER;
        public static DebugCommand<int> SET_SCORE;
        public static DebugCommand KILL_PLAYER;

        public List<object> commandList;

        private void OnEnable(){
            DevConsoleInputMap.Enable();

            Actions[0].started += _ => ToggleConsole();
            Actions[1].started += _ => OnEnterPressed();
        }
        private void OnDisable(){
            DevConsoleInputMap.Disable();
        }

        private void Awake(){

            SPAWN_PLAYER = new DebugCommand("spawn_player", "Spawns a player into the scene.", "spawn_player", () => {
                //GameManager.Instance.SpawnPlayer();
            });

            SET_SCORE = new DebugCommand<int>("set_gold", "Sets the player score to given amount", "set_gold <score_amount>", (x) => {
                Debug.Log(x);
            });

            KILL_PLAYER = new DebugCommand("kill_player", "Kills the active player on the scene", "kill_player", () => {
                //GameManager.Instance.DestroyPlayer();
            });

            commandList = new List<object> {
                SPAWN_PLAYER,
                SET_SCORE,
                KILL_PLAYER
            };
        }
        private void ToggleConsole() => showConsole = !showConsole;
        private void OnEnterPressed(){
            if (!showConsole) return;
            HandleInput();
            input = "";
        }

        private void OnGUI(){
            if (!showConsole) return;  

            float y = 0f;

            GUI.Box(new Rect(0, y, Screen.width, 70), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            var style = new GUIStyle { fontSize = 50, fontStyle = FontStyle.Bold };
            input = GUI.TextField(new Rect(20f, y + 10f, Screen.width - 20f, 500f), input, style);
        }
        private void HandleInput(){

            string[] properties = input.Split(' ');
            
            for (int i = 0; i < commandList.Count; i++) {

                if (commandList[i] is DebugCommandBase commandBase && input.Contains(commandBase.commandId)) {
                    if (commandList[i] is DebugCommand) {
                        (commandList[i] as DebugCommand)?.Invoke();
                    }
                    else if (commandList[i] is DebugCommand<int>) {
                        (commandList[i] as DebugCommand<int>)?.Invoke(int.Parse(properties[1]));
                    }
                }
            }
            showConsole = false;
        }
    }
}
