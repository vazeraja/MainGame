using UnityEngine;

namespace MainGame {

    public class Singleton : MonoBehaviour {

        private static Singleton instance;
        public static Singleton Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<Singleton>();
                    if (instance == null) {
                        var go = new GameObject("Singleton");
                        instance = go.AddComponent<Singleton>();
                    }
                }
                return instance;
            }
        }
        private void Awake(){
            if (Instance != this) {
                Debug.Log($"Singleton Duplicate: deleting {this.name}", this);
                DestroyImmediate(gameObject);
            }
            else {
                Debug.Log($"Singleton: {this.name}", this);
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
                #endif
                DontDestroyOnLoad(gameObject);
            }
        }
        private void OnDestroy(){
            if (instance == this) {
                Debug.Log("Singleton OnDestroy: set instance to null");
                instance = null;
            }
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            #endif
        }
        #if UNITY_EDITOR
        private void EditorApplication_playModeStateChanged(UnityEditor.PlayModeStateChange obj){
            switch (obj) {
                case UnityEditor.PlayModeStateChange.ExitingEditMode:
                    Debug.Log("Singleton PlayModeStateChange ExitingEditMode: set instance to null");
                    instance = null;
                    break;
            }
        }
        #endif
    }
}
