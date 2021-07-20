using System.Collections.Generic;
using TN.SaveSystem;
using UnityEngine;

public class JsonSaveService {

    private IEnumerable<ISaveable> saveables;

    public static void SaveJsonData(IEnumerable<ISaveable> saveables)
    {
        SaveData sd = new SaveData();
        
        foreach (var saveable in saveables)
        {
            saveable.PopulateSaveData(sd);
        }

        if (FileSaveLoader.WriteToFile("SaveData01.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(IEnumerable<ISaveable> saveables) {
        if (FileSaveLoader.LoadFromFile("SaveData01.dat", out var json)) {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            foreach (var saveable in saveables)
            {
                saveable.LoadFromSaveData(sd);
            }

            Debug.Log("Load complete");
        }
    }
}