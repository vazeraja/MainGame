using System;
using UnityEngine;

namespace ThunderNut.GameEngine {
    
    public class App {
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() {
            var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
            if (app == null) {
                throw new ApplicationException();
            }
            app.name = "App";
            
            UnityEngine.Object.DontDestroyOnLoad(app);
        }
    }
}