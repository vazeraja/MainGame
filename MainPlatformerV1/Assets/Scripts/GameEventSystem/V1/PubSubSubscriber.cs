using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainGame {

    public class PubSubSubscriber : MonoBehaviour {

        public string EventName;
        
        [Serializable]
        public class BoolEvent : UnityEvent<bool> {}
        public BoolEvent OnBoolMessageReceived;

        private void Awake(){
            PubSubBroker.Instance.Subscribe(EventName, MessageReceived);
        }
        private void OnDestroy(){  
            PubSubBroker.Instance.Unsubscribe(EventName, MessageReceived);
        }
        public void MessageReceived(PubSubBroker.PubSubMessage message){
            OnBoolMessageReceived.Invoke(message.GetValue<bool>());
        }

    }
}
