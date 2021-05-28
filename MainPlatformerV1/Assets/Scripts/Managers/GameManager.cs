using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace MainGame {
    public class GameManager : MonoBehaviour {

        [SerializeField] private PlayerData playerData = null;
        
        private MainPlayer activePlayer = null;

        private readonly Vector3 spawnPoint = new Vector3(-9f, 1f, 0f);

        public void RegisterPlayer(MainPlayer mainPlayer){
            Debug.Log("player spawned");
            activePlayer = mainPlayer;
        }
        public void SpawnPlayer(){
            var localToWorldMatrix = transform.localToWorldMatrix;

            var playerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), (localToWorldMatrix * spawnPoint), Quaternion.identity);
            playerPrefab.name = "Player";

            var cam = Instantiate(Resources.Load<GameObject>("Prefabs/CinemachineVCam"), (localToWorldMatrix * new Vector4(0, 0, 0, 0)), Quaternion.identity);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = playerPrefab.transform;
        }
        public void DestroyPlayer() => Destroy(activePlayer.gameObject);

        private void OnApplicationQuit(){}
    }

}
