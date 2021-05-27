using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    // [CreateAssetMenu(fileName = "GameManager", menuName = "Game Manager", order = 0)]
    // public class GameMaster : SingletonScriptableObject<GameMaster> {
    //
    //     /// <summary>
    //     /// Optional:
    //     /// This method will run before the first scene is loaded. Initializing the singleton here
    //     /// will allow it to be ready before any other GameObjects on every scene and will prevent the "initialization on first usage". 
    //     /// </summary>
    //     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //     public static void BeforeSceneLoad(){ BuildSingletonInstance(); }
    //     
    //     /// <summary>
    //     /// Optional:
    //     /// Will run when the Singleton Scriptable Object is first created on the assets. 
    //     /// Usually this happens on edit mode, not runtime. (the override keyword is mandatory for this to work)
    //     /// </summary>
    //     protected override void ScriptableObjectAwake(){
    //         Debug.Log(GetType().Name + " created.");
    //     }
    //     public override void Start(){
    //         base.Start();
    //     }
    //     public override void MonoBehaviourAwake(){
    //         base.MonoBehaviourAwake();
    //     }
    //     public override void Update(){
    //         base.Update();
    //     }
    //     public override void FixedUpdate(){
    //         base.FixedUpdate();
    //     }
    //     
    //
    // }
}
