using System;
using Cinemachine;
using UnityEngine;

namespace MainGame {

    public class AutoSetPlayer : MonoBehaviour {
        
        public CinemachineVirtualCamera cam;
        public GameEventListenerSO<MainPlayer, PlayerEvent, UnityPlayerEvent> playerListener;

        public Optional<MainPlayer> target;

        private void Awake(){
            playerListener.UnityEvent.AddListener(SetPlayer);
        }
        private void SetPlayer(MainPlayer player){
            if (!target.Enabled) return;

            Debug.Log("Cinemachine: Follow target assigned to player");
            target.Value = player;
            cam.Follow = target.Value.transform;
        }
    }
}
