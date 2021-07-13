using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ThunderNut.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingLoadingExample : MonoBehaviour {

    private string SavePath => $"./save.data";
    
    [ContextMenu("Save")]
    private void Save() {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    private void Load() {
        var state = LoadFile();
        RestoreState(state);
    }
    
    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (var stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }
    
    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<Saveable>())
        {
            state[saveable.ID] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<Saveable>())
        {
            if (state.TryGetValue(saveable.ID, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
    
}