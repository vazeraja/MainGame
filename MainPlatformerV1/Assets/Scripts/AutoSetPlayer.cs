using System;
using Cinemachine;
using UnityEngine;

namespace MainGame {

    public class AutoSetPlayer : MonoBehaviour {
        
        public CinemachineVirtualCamera cam;
        public GameEventListenerSO<Player, PlayerEvent, UnityPlayerEvent> playerListener;

        public Optional<Player> target;

        private void Awake(){
            playerListener.UnityEvent.AddListener(SetPlayer);
        }
        private void SetPlayer(Player player){
            if (!target.Enabled) 
                return;
            
            target.Value = player;
            cam.Follow = target.Value.transform;
            
            Debug.Log("<b><color=white>Cinemachine: Follow target assigned to player </color></b>");
        }
    }
}
