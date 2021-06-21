


using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PluggableStateMachine))]
public class PluggableStateMachineEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor Window")) {
            PluggableStateMachineEditorWindow.Open((PluggableStateMachine) target);
        }
    }
}