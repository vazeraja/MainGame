using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace MainGame {
    public class GameManager : MonoBehaviour {

        [SerializeField] private PlayerData playerData = null;

        private PlayerEventListener playerEventListener;
        private MainPlayer activeMainPlayer = null;

        private readonly Vector3 spawnPoint = new Vector3(-9f, 1f, 0f);

        private void Awake(){
            playerEventListener = GetComponent<PlayerEventListener>();
            playerEventListener.UnityEventResponse.AddListener(RegisterPlayer);
        }

        public void RegisterPlayer(MainPlayer mainPlayer){
            Debug.Log("player spawned");
            activeMainPlayer = mainPlayer;
        }
        public void SpawnPlayer(){
            var localToWorldMatrix = transform.localToWorldMatrix;

            var playerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), (localToWorldMatrix * spawnPoint), Quaternion.identity);
            playerPrefab.name = "Player";

            var cam = Instantiate(Resources.Load<GameObject>("Prefabs/CinemachineVCam"), (localToWorldMatrix * new Vector4(0, 0, 0, 0)), Quaternion.identity);
            cam.GetComponent<CinemachineVirtualCamera>().Follow = playerPrefab.transform;
        }
        public void DestroyPlayer() => Destroy(activeMainPlayer.gameObject);

        #region Save System
        // private void SaveJsonData(){
        //     var sd = new SaveData();
        //     PopulateSaveData(sd);
        //
        //     if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        //         Debug.Log("Save Successful");
        //
        // }
        // public void PopulateSaveData(SaveData saveData){
        //     saveData.m_Score = playerData.currentScore;
        // }
        //
        // private static void LoadJsonData(GameManager gameManager){
        //     if (FileManager.LoadFromFile("SaveData.dat", out string json)) {
        //         var sd = new SaveData();
        //         sd.LoadFromJson(json);
        //
        //         LoadFromSaveData(sd);
        //         Debug.Log("Load Complete");
        //     }
        // }
        //
        // public void LoadFromSaveData(SaveData saveData){
        //     playerData.currentScore = saveData.m_Score;
        // }
        #endregion

        private void OnApplicationQuit(){}
    }

}
