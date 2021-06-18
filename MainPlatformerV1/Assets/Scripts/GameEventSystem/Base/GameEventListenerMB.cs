using UnityEngine;
using UnityEngine.Events;

namespace MainGame {

    public interface IGameEventListener<in T> {
        void OnEventRaised(T item);
    }
    
    // Create a MonoBehaviour and attach to a game object
    public abstract class GameEventListenerMB<TType, TGameEvent, TUnityEvent> : MonoBehaviour,
        IGameEventListener<TType> where TGameEvent : BaseGameEvent<TType> where TUnityEvent : UnityEvent<TType> {

        [SerializeField] protected TGameEvent gameEvent;
        [SerializeField] protected TUnityEvent unityEventResponse;

        private void OnEnable(){
            if (gameEvent == null) return;
            gameEvent.RegisterListener(this);
        }
        private void OnDisable(){
            if (gameEvent == null) return;
            gameEvent.UnregisterListener(this);
        }
        public void OnEventRaised(TType item) => unityEventResponse?.Invoke(item);
    }
    
    // Create a scriptable object listener
    public class GameEventListenerSO<TType, TGameEvent, TUnityEvent> : ScriptableObject,
        IGameEventListener<TType> where TGameEvent : BaseGameEvent<TType> where TUnityEvent : UnityEvent<TType> {

        [SerializeField] private TGameEvent gameEvent;
        [SerializeField] private TUnityEvent unityEvent;

        public TGameEvent GameEvent { get => gameEvent; set => gameEvent = value; }
        public TUnityEvent UnityEvent { get => unityEvent; set => unityEvent = value; }

        private void OnEnable(){
            if (GameEvent == null) return;
            GameEvent.RegisterListener(this);
        }
        private void OnDisable(){
            if (GameEvent == null) return;
            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(TType item){
            UnityEvent?.Invoke(item);
        }
    }
    
    // Directly create listener instance and register the GameEvent and UnityEvent manually
    [System.Serializable]
    public class GameEventListener<TType, TGameEvent, TUnityEvent> :
        IGameEventListener<TType> where TGameEvent : BaseGameEvent<TType> where TUnityEvent : UnityEvent<TType> {
        
        [SerializeField] private TGameEvent gameEvent;
        [SerializeField] private TUnityEvent unityEvent;

        public TGameEvent GameEvent { get => gameEvent; set => gameEvent = value; }
        public TUnityEvent UnityEvent { get => unityEvent; set => unityEvent = value; }

        public void Register(){
            if (GameEvent == null) return;
            GameEvent.RegisterListener(this);
        }
        public void Unregister(){
            if (GameEvent == null) return;
            GameEvent.UnregisterListener(this);
        }
        
        public void OnEventRaised(TType item){
            UnityEvent?.Invoke(item);
        }
    }
    

}
