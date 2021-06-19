using System;
using MainGame;
using UnityEditor;
using UnityEngine;


public class SasukeBaseStateEditorWindow : ExtendedEditorWindow {
    private SerializedObject serializedObject;
    
    public static void Open(SasukeStateSO baseState) {
        SasukeBaseStateEditorWindow baseStateWindow = GetWindow<SasukeBaseStateEditorWindow>("Base State Editor");
        baseStateWindow.serializedObject = new SerializedObject(baseState);
    }

    private void OnGUI() {
        var stateName = serializedObject.FindProperty("stateName");
        var states = serializedObject.FindProperty("states");
        var transitions = serializedObject.FindProperty("transitions");
        DrawProperties(stateName, true);
        DrawProperties(states, true);
        DrawProperties(transitions, true);
    }
}