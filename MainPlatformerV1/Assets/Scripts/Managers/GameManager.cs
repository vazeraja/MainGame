using System;
using System.Collections.Generic;
using MainGame.DeveloperConsole;
using MainGame.Utils;
using UnityEngine;
using static MainGame.DeveloperConsole.DeveloperConsoleBehaviour;

namespace MainGame {
    public class GameManager : MonoBehaviour {

        private InputHandler inputHandler;

        private void Awake() {
            inputHandler = gameObject.AddComponent<InputHandler>();
        }
        
    }

}
