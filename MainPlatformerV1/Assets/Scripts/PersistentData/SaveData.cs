using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [System.Serializable]
    public class SaveData {
        public float m_Score;

        public string ToJson() => JsonUtility.ToJson(this);
        public void LoadFromJson(string a_Json) => JsonUtility.FromJsonOverwrite(a_Json, this);
    }
    public interface ISaveable {
        void PopulateSaveData(SaveData a_Savedata);
        void LoadFromSaveData(SaveData a_SaveData);
    }
}