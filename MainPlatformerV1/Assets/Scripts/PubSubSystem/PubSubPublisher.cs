using System;
using UnityEngine;
using UnityEngine.UI;


namespace MainGame {
    public class PubSubPublisher : MonoBehaviour {

        private Button button;
        public string EventName;

        private void Awake(){
            button = GetComponent<Button>();
        }
        public void PublishBoolean(bool value){
            PubSubBroker.Instance.Publish(EventName, this, value);
        }

        
        
    }
}
