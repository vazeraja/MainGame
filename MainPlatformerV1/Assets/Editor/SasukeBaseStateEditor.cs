using MainGame;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class AssetHandler {
    
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line) {
        
        SasukeStateSO obj = EditorUtility.InstanceIDToObject(instanceId) as SasukeStateSO;
        if (obj == null) return false;
        SasukeBaseStateEditorWindow.Open(obj);
        return true;
    }
}

[CustomEditor(typeof(SasukeStateSO))]
public class SasukeBaseStateEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SasukeStateSO baseState = (SasukeStateSO) target;
        if (GUILayout.Button("Open Editor")) {
            SasukeBaseStateEditorWindow.Open(baseState);
        }
    }
}