using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame {
    public class PubSubBroker : MonoBehaviour {

        // Lazy loading: Doesnt create the singleton instance until you request for one
        private static PubSubBroker instance;
        public static PubSubBroker Instance => Equals(instance, null) ? instance = CreateNewBroker() : instance;
        private static PubSubBroker CreateNewBroker(){
            var gameObject = new GameObject("PubSubBroker");
            return gameObject.AddComponent<PubSubBroker>();
        }

        public struct PubSubMessage {
            public string EventName;
            public object Sender;
            public object Value;
            public T GetValue<T>(){
                if (Value is T) return (T)Value;
                return default(T);
            }
            public Component GetSenderAsComponent(){
                if (Sender is Component) return (Component)Sender;
                return null;
            }
        }

        private readonly Dictionary<string, List<Action<PubSubMessage>>> EventLibrary = new Dictionary<string, List<Action<PubSubMessage>>>();
        public void Subscribe(string eventName, Action<PubSubMessage> callback){
            if (!EventLibrary.ContainsKey(eventName)) {
                EventLibrary.Add(eventName, new List<Action<PubSubMessage>>());
            }
            EventLibrary[eventName].Add(callback);
        }
        public void Unsubscribe(string eventName, Action<PubSubMessage> callback){
            if (!EventLibrary.ContainsKey(eventName)) return;
            EventLibrary[eventName].Remove(callback);
        }
        public void Publish<T>(string eventName, object sender, T value){
            if (!EventLibrary.ContainsKey(eventName)) {
                Debug.LogWarning($"PubSubBroker: Event Name '{eventName}' not found");
                return;
            }
            var message = new PubSubMessage() {
                EventName = eventName,
                Sender = sender,
                Value = value
            };
            foreach (var callback in EventLibrary[eventName]) {
                try {
                    callback.Invoke(message);
                }
                catch (Exception ex) {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
