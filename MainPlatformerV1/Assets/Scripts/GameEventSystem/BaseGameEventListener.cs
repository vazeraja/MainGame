using UnityEngine;
using UnityEngine.Events;

namespace MainGame {
    
    public interface IGameEventListener<in T> {
        void OnEventRaised(T item);
    }

    public abstract class BaseGameEventListener<TType, TEvent, TUer> : MonoBehaviour,
        IGameEventListener<TType> where TEvent : BaseGameEvent<TType> where TUer : UnityEvent<TType> {

        [SerializeField] private TEvent gameEvent;
        public TEvent GameEvent { get => gameEvent; set => gameEvent = value; }

        [SerializeField] private TUer unityEventResponse;

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
