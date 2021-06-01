using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MainGame {

    public class AutoSetPlayer : MonoBehaviour {

        private Optional<MainPlayer> activePlayer = new Optional<MainPlayer>(false);
        public CinemachineVirtualCamera cam;

        public GameEventListenerSO<MainPlayer, PlayerEvent, UnityPlayerEvent> playerListener;
        
        // [SerializeField] private GameEventListenerT2<MainPlayer, PlayerEvent, UnityPlayerEvent> playerListener;
        private void OnEnable(){
            // playerListener.Register();
            playerListener.UnityEvent.AddListener((x) => { cam.Follow = x.transform; activePlayer.Enabled = true; });
        }
    }
}
