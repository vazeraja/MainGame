using System;
using ThunderNut.StateMachine;
using UnityEngine;

namespace TN.GameEngine {
    
    public static class App {
        
        // public GameModeManager GameModeManager;
        // public SaveManager SaveManager;
        // public InputManager InputManager;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() {
            var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
            if (app == null) {
                throw new ApplicationException();
            }
            app.name = "App";
            
            UnityEngine.Object.DontDestroyOnLoad(app);
        }

        // public static App CreateApp() {
        //     var appPrefab = GameObject.FindGameObjectWithTag("App");
// 
        //     App app = new App {
        //         GameModeManager = appPrefab.GetComponent<GameModeManager>(),
        //         SaveManager = appPrefab.GetComponent<SaveManager>(),
        //         InputManager = appPrefab.GetComponent<InputManager>()
        //     };
        //     
        //     return app;
        // }
    }
}