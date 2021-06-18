using UnityEngine;

namespace MainGame {
    public class RuntimeSingleton<T> : MonoBehaviour
        where T : RuntimeSingleton<T> {
        // The singleton instance.
        public static T Instance => _instance != null ? _instance :
            Application.isPlaying ? Initialize() : null;
        static T _instance;

        // True if the singleton instance has been destroyed. Used to prevent possible
        // re-creation of the singleton when exiting.
        static bool _destroyed;

        // Finds or creates the singleton instance and stores it in _instance. This can
        // be called from a derived type to ensure creation of the singleton using the 
        // [RuntimeInitializeOnLoadMethod] attribute on a static method.
        private static T Initialize(){
            // Prevent re-creation of the singleton during play mode exit.
            if (_destroyed) return null;

            // If the instance is already valid, return it. Needed if called from a
            // derived class that wishes to ensure the instance is initialized.
            if (_instance != null) return _instance;

            // Find the existing instance (across domain reloads).
            if ((_instance = FindObjectOfType<T>()) != null) return _instance;

            // Create a new GameObject instance to hold the singleton component.
            var gameObject = new GameObject(typeof(T).Name);

            // Move the instance to the DontDestroyOnLoad scene to prevent it from
            // being destroyed when the current scene is unloaded.
            DontDestroyOnLoad(gameObject);

            // Create the MonoBehavior component. Awake() will assign _instance.
            return gameObject.AddComponent<T>();
        }

        // Called when the instance is created.
        private void Awake(){
            // Verify there is not more than one instance and assign _instance.
            Debug.Assert(_instance == null,
                "More than one singleton instance instantiated!", this);
            _instance = (T)this;
        }

        // Clear the instance field when destroyed and prevent it from being re-created.
        private void OnDestroy(){
            _instance = null;
            _destroyed = true;
        }

        // Called when the singleton is created *or* after a domain reload in the editor.
        private void OnEnable(){
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            #endif
        }

        #if UNITY_EDITOR
        // Called when entering or exiting play mode.
        static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange stateChange){
            // Reset static _destroyed field. Required when domain reloads are disabled.
            // Note: ExitingPlayMode is called too early.
            if (stateChange == UnityEditor.PlayModeStateChange.EnteredEditMode) {
                UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                _destroyed = false;
            }
        }
        #endif
    }
}
