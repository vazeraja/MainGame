using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PluggableStateMachineEditorWindow : EditorWindow {
    private SerializedObject serializedObject;

    private bool retrieved = false;
    private readonly List<SerializedObject> stateObjects = new List<SerializedObject>();
    private readonly List<SerializedProperty> transitionsList = new List<SerializedProperty>();

    public static void Open(PluggableStateMachine basePluggableState) {
        PluggableStateMachineEditorWindow window =
            GetWindow<PluggableStateMachineEditorWindow>("State Machine Editor Window");
        window.serializedObject = new SerializedObject(basePluggableState);
    }

    private void GetAll() {
        var statesList = serializedObject.FindProperty("states");
        for (int i = 0; i < statesList.arraySize; i++) {
            var state = statesList.GetArrayElementAtIndex(i);

            var stateObject = new SerializedObject(state.objectReferenceValue as ScriptableObject);
            if (!stateObjects.Contains(stateObject)) stateObjects.Add(stateObject);

            var transition = stateObject.FindProperty("transitions");
            if (!transitionsList.Contains(transition)) transitionsList.Add(transition);
        }

        retrieved = true;
    }

    private void OnGUI() {
        if (!retrieved) GetAll();

        GUILayout.Label("Transition Table", EditorStyles.boldLabel);
        EditorGUILayout.Space(10, true);

        EditorGUILayout.BeginHorizontal();

        for (var index = 0; index < stateObjects.Count; index++) {
            var state = stateObjects[index];
            
            var stateName = state.FindProperty("stateName");
            var states = state.FindProperty("states");
            var stateTransition = state.FindProperty("transitions");

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            
            stateName.stringValue = EditorGUILayout.TextField(stateName.stringValue);
            
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(states);

            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(stateTransition);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();


        stateObjects.ForEach(x => x.ApplyModifiedProperties());
        serializedObject.ApplyModifiedProperties();
    }
}