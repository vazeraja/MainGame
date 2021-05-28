using UnityEngine;
using UnityEngine.Events;

namespace MainGame {
    
    public interface IGameEventListener<in T> {
        void OnEventRaised(T item);
    }

    public abstract class GameEventListener<TType, TGameEvent, TUnityEvent> : MonoBehaviour,
        IGameEventListener<TType> where TGameEvent : BaseGameEvent<TType> where TUnityEvent : UnityEvent<TType> {

        [SerializeField] private TGameEvent gameEvent;
        public TGameEvent GameEvent { get => gameEvent; set => gameEvent = value; }

        [SerializeField] private TUnityEvent unityEventResponse;
        public TUnityEvent UnityEventResponse { get => unityEventResponse; set => unityEventResponse = value; }

        private void OnEnable(){
            if (gameEvent == null) return;
            GameEvent.RegisterListener(this);
        }
        private void OnDisable(){
            if (gameEvent == null) return;
            GameEvent.UnregisterListener(this);
        }
        public void OnEventRaised(TType item) => unityEventResponse?.Invoke(item);
    }
}
