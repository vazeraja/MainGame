using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public class GameManager : Singleton<GameManager>, ISaveable {
        protected GameManager() { } // (Optional) Prevent non-singleton constructor use.

        [SerializeField] private PlayerData playerData = null;
        public PlayerData PlayerData => playerData;

        private void Start() {
            LoadJsonData(this);
        }

        private void OnApplicationQuit() => SaveJsonData(this);

        private static void SaveJsonData(GameManager gameManager) {
            SaveData sd = new SaveData();
            gameManager.PopulateSaveData(sd);

            if (FileManager.WriteToFile("SaveData.dat", sd.ToJson())) {
                Debug.Log("Save Successful");
            }

        }
        public void PopulateSaveData(SaveData a_Savedata) {
            a_Savedata.m_Score = playerData.currentScore;
        }

        private static void LoadJsonData(GameManager gameManager) {
            if (FileManager.LoadFromFile("SaveData.dat", out var json)) {
                SaveData sd = new SaveData();
                sd.LoadFromJson(json);

                gameManager.LoadFromSaveData(sd);
                Debug.Log("Load Complete");
            }
        }

        public void LoadFromSaveData(SaveData a_SaveData) {
            playerData.currentScore = a_SaveData.m_Score;
        }

    }

}
