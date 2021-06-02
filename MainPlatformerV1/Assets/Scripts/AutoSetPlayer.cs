using System;
using Cinemachine;
using UnityEngine;

namespace MainGame {

    public class AutoSetPlayer : MonoBehaviour {
        
        private CinemachineVirtualCamera cam;
        public GameEventListenerSO<MainPlayer, PlayerEvent, UnityPlayerEvent> playerListener;

        public Optional<MainPlayer> target;

        private void OnEnable(){
            cam = GetComponent<CinemachineVirtualCamera>();
            if(target.Enabled) playerListener.UnityEvent.AddListener((x) => { target.Value = x; cam.Follow = x.transform;});
        }
    }
}
