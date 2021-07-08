using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ThunderNut.SceneManagement {
    [CustomEditor(typeof(SceneHandle))]
    public class SceneHandleEditor : Editor {
        // This will be the serialized clone property of SceneHandle.CharacterList
        private SerializedProperty SceneTagsProperty;

        // This will be the serialized clone property of SceneHandle.DialogueItems
        private SerializedProperty PassageElementsProperty;

        // This will be the serialized clone property of SceneHandle.scene
        private SerializedProperty sceneProp;

        // You have to implement completely custom behavior of how to display and edit 
        // the list elements
        private ReorderableList charactersList;
        private ReorderableList passageElementsList;

        // Reference to the actual Dialogue instance this Inspector belongs to
        private SceneHandle sceneHandle;

        // class field for storing available options
        private GUIContent[] availableOptions;

        // Called when the Inspector is opened / ScriptableObject is selected
        private void OnEnable() {
            Debug.Log("OnEnable");
            // Get the target as the type you are actually using
            sceneHandle = (SceneHandle) target;

            // Link in serialized fields to their according SerializedProperties
            SceneTagsProperty = serializedObject.FindProperty(nameof(SceneHandle.sceneTags));
            PassageElementsProperty = serializedObject.FindProperty(nameof(SceneHandle.passageElements));

            // Setup and configure the charactersList we will use to display the content of the CharactersList 
            // in a nicer way
            charactersList = new ReorderableList(serializedObject, SceneTagsProperty) {
                displayAdd = true,
                displayRemove = true,
                draggable = false, // for now disable reorder feature since we later go by index!

                // As the header we simply want to see the usual display name 
                drawHeaderCallback = rect => {
                    GUIStyle style = new GUIStyle(EditorStyles.label) {
                        normal = {textColor = Color.black}
                    };

                    EditorGUI.LabelField(rect, SceneTagsProperty.displayName, style);
                },

                // How shall elements be displayed
                drawElementCallback = (rect, index, focused, active) => {
                    // get the current element's SerializedProperty
                    var element = SceneTagsProperty.GetArrayElementAtIndex(index);

                    // Get all characters as string[]
                    var availableIDs = sceneHandle.sceneTags;

                    // store the original GUI.color
                    var color = GUI.color;
                    // Tint the field in red for invalid values
                    // either because it is empty or a duplicate
                    if (string.IsNullOrWhiteSpace(element.stringValue) ||
                        availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                        GUI.color = new Color(0.69f, 0.41f, 0.18f);
                    }

                    // Draw the property which automatically will select the correct drawer -> a single line text field
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)),
                        element);

                    // reset to the default color
                    GUI.color = color;

                    // If the value is invalid draw a HelpBox to explain why it is invalid
                    if (string.IsNullOrWhiteSpace(element.stringValue)) {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                            "ID may not be empty!", MessageType.Error);
                    }
                    else if (availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                            "Duplicate! ID has to be unique!", MessageType.Error);
                    }
                },

                // Get the correct display height of elements in the list according to their values
                elementHeightCallback = index => {
                    var element = SceneTagsProperty.GetArrayElementAtIndex(index);
                    var availableIDs = sceneHandle.sceneTags;

                    var height = EditorGUI.GetPropertyHeight(element);

                    if (string.IsNullOrWhiteSpace(element.stringValue) ||
                        availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                        height += EditorGUIUtility.singleLineHeight;
                    }

                    return height;
                },

                // Overwrite what shall be done when an element is added via the +
                // Reset all values to the defaults for new added elements
                // By default Unity would clone the values from the last or selected element otherwise
                onAddCallback = list => {
                    // This adds the new element but copies all values of the select or last element in the list
                    list.serializedProperty.arraySize++;

                    var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.stringValue = "";
                }
            };

            // Set up PassageElements List as Reorderable list
            passageElementsList = new ReorderableList(serializedObject, PassageElementsProperty) {
                displayAdd = true,
                displayRemove = true,
                draggable = true,

                // As the header we simply want to see the usual display name of the DialogueItems
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, PassageElementsProperty.displayName),

                // How shall elements be displayed
                drawElementCallback = (rect, index, focused, active) => {
                    // get the current element's SerializedProperty
                    var element = PassageElementsProperty.GetArrayElementAtIndex(index);

                    // Get the nested property fields of the DialogueElement class
                    var sceneTag = element.FindPropertyRelative(nameof(PassageElement.sceneTag));
                    var handle = element.FindPropertyRelative(nameof(PassageElement.sceneHandle));
                    var handleTags = element.FindPropertyRelative(nameof(PassageElement.sceneHandleTags));

                    var subHandleTags = handle.FindPropertyRelative(nameof(SceneHandle.sceneTags));

                    var popUpHeight = EditorGUI.GetPropertyHeight(sceneTag);

                    // store the original GUI.color
                    var color = GUI.color;

                    // if the value is invalid tint the next field red
                    if (sceneTag.intValue < 0) GUI.color = Color.red;

                    // Draw the Popup so you can select from the existing character names
                    sceneTag.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight),
                        new GUIContent(sceneHandle.scene != null ? sceneHandle.scene.name : sceneTag.displayName),
                        sceneTag.intValue, availableOptions);

                    // reset the GUI.color
                    GUI.color = color;
                    rect.y += popUpHeight;

                    var handleHeight = EditorGUI.GetPropertyHeight(handle);

                    if (handle.objectReferenceValue == null) {
                        GUI.color = Color.red;
                    }
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, handleHeight), handle,
                        new GUIContent("Target Scene"));

                    // reset the GUI.color
                    GUI.color = color;
                    rect.y += popUpHeight;

                    // Draw the text field
                    // since we use a PropertyField it will automatically recognize that this field is tagged [TextArea]
                    // and will choose the correct drawer accordingly
                    var textHeight = EditorGUI.GetPropertyHeight(subHandleTags);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, textHeight), subHandleTags);

                    // var handleOptions = sceneHandle.passageElements[index].sceneHandle.sceneTags.Select(item => new GUIContent(item)).ToArray();
                    
                    // handleTags.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight),
                        // new GUIContent("Passage"), handleTags.intValue, new GUIContent[] { });
                },

                // Get the correct display height of elements in the list
                // according to their values
                // in this case e.g. we add an additional line as a little spacing between elements
                elementHeightCallback = index => {
                    var element = PassageElementsProperty.GetArrayElementAtIndex(index);

                    var sceneTag = element.FindPropertyRelative(nameof(PassageElement.sceneTag));
                    var handle = element.FindPropertyRelative(nameof(PassageElement.sceneHandle));
                    var handleTags = element.FindPropertyRelative(nameof(PassageElement.sceneHandleTags));

                    return EditorGUI.GetPropertyHeight(sceneTag) + EditorGUI.GetPropertyHeight(handle) +
                           EditorGUI.GetPropertyHeight(handleTags) +
                           EditorGUIUtility.singleLineHeight;
                },

                // Overwrite what shall be done when an element is added via the +
                // Reset all values to the defaults for new added elements
                // By default Unity would clone the values from the last or selected element otherwise
                onAddCallback = list => {
                    // This adds the new element but copies all values of the select or last element in the list
                    list.serializedProperty.arraySize++;

                    var newElement =
                        list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    var sceneTag = newElement.FindPropertyRelative(nameof(PassageElement.sceneTag));
                    var handle = newElement.FindPropertyRelative(nameof(PassageElement.sceneHandle));
                    var text = newElement.FindPropertyRelative(nameof(PassageElement.sceneHandleTags));

                    sceneTag.intValue = -1;
                },
            };

            // Get the existing character names ONCE as GuiContent[]
            // Later only update this if the List was changed
            availableOptions = sceneHandle.sceneTags.Select(item => new GUIContent(item)).ToArray();
        }

        public override void OnInspectorGUI() {
            DrawScriptField();

            // load real target values into SerializedProperties
            serializedObject.Update();

            sceneProp = serializedObject.FindProperty("scene");
            EditorGUILayout.PropertyField(sceneProp);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            charactersList.DoLayoutList();
            if (EditorGUI.EndChangeCheck()) {
                // Write back changed values into the real target
                serializedObject.ApplyModifiedProperties();

                // Update the existing names as GuiContent[]
                availableOptions = sceneHandle.sceneTags.Select(item => new GUIContent(item)).ToArray();

                EditorUtility.SetDirty(target);
            }

            passageElementsList.DoLayoutList();

            // Write back changed values into the real target
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((SceneHandle) target),
                typeof(SceneHandle), false);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
        }

        [MenuItem("Assets/Create/World Graph/Scene Handle (From Scene)", false, 400)]
        private static void CreateFromScene() {
            var trailingNumbersRegex = new Regex(@"(\d+$)");

            var scene = Selection.activeObject as SceneAsset;

            var asset = CreateInstance<SceneHandle>();
            asset.scene = scene;
            string baseName = trailingNumbersRegex.Replace(scene != null ? scene.name : string.Empty, "");
            asset.name = baseName + "Handle";

            string assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(scene));
            AssetDatabase.CreateAsset(asset, Path.Combine(assetPath ?? Application.dataPath, asset.name + ".asset"));
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/Create/World Graph/Scene Handle (From Scene)", true, 400)]
        private static bool CreateFromSceneValidation() => Selection.activeObject as SceneAsset;
    }
}