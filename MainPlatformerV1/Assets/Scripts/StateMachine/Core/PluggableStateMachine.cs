using System.Collections.Generic;
using System.Linq;
using MainGame;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PluggableStateMachine : MonoBehaviour {
    [SerializeField] public List<BaseState<SasukeController, SasukeStateSO>> statesList;
}

// #if UNITY_EDITOR
// [CustomEditor(typeof(PluggableStateMachine))]
// public class StateMachineTransitionTableEditor : Editor {
//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();
//         if (GUILayout.Button("Open Editor Window")) {
//             StateMachineTransitionTableEditorWindow.Open((PluggableStateMachine) target);
//         }
//     }
// }
//
// public class StateMachineTransitionTableEditorWindow : EditorWindow {
//     private SerializedObject serializedObject;
//
//     private bool retrieved = false;
//     private readonly List<SerializedObject> stateObjects = new List<SerializedObject>();
//     private readonly List<SerializedProperty> transitionsList = new List<SerializedProperty>();
//
//     public static void Open(PluggableStateMachine basePluggableState) {
//         StateMachineTransitionTableEditorWindow window =
//             GetWindow<StateMachineTransitionTableEditorWindow>("Base State Editor");
//         window.serializedObject = new SerializedObject(basePluggableState);
//     }
//
//     private void GetAll() {
//         var statesList = serializedObject.FindProperty("statesList");
//         for (int i = 0; i < statesList.arraySize; i++) {
//             var state = statesList.GetArrayElementAtIndex(i);
//
//             var stateObject = new SerializedObject(state.objectReferenceValue as ScriptableObject);
//             if (!stateObjects.Contains(stateObject)) stateObjects.Add(stateObject);
//
//             var transition = stateObject.FindProperty("transitions");
//             if (!transitionsList.Contains(transition)) transitionsList.Add(transition);
//         }
//
//         retrieved = true;
//     }
//
//     private void OnGUI() {
//         if (!retrieved) GetAll();
//
//         GUILayout.Label("Transition Table", EditorStyles.boldLabel);
//         EditorGUILayout.Space(10, true);
//
//         EditorGUILayout.BeginHorizontal();
//
//         foreach (var state in stateObjects) {
//             var stateName = state.FindProperty("stateName");
//             var stateTransition = state.FindProperty("transitions");
//
//             EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
//
//             EditorGUILayout.LabelField("State Name", GUILayout.MaxWidth(100));
//             stateName.stringValue = EditorGUILayout.TextField(stateName.stringValue);
//             EditorGUILayout.PropertyField(stateTransition, GUIContent.none);
//
//             EditorGUILayout.EndVertical();
//         }
//
//         EditorGUILayout.EndHorizontal();
//
//
//         stateObjects.ForEach(x => x.ApplyModifiedProperties());
//         serializedObject.ApplyModifiedProperties();
//     }
// }
//
// #endif