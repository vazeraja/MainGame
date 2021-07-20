using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public string ToJson() => JsonUtility.ToJson(this);
    public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);

    [System.Serializable] public struct PlayerProfile {
        public Vector3 location;
    }

    public PlayerProfile profile;
    
}
public interface ISaveable {
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData(SaveData saveData);
}