using MainGame;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(State<>))]
public class PluggableStatePropertyDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        
        float width = (position.width - 20);
        
        position.width = width;
        EditorGUI.PropertyField(position, property, GUIContent.none);
        
        
        EditorGUI.EndProperty();
    }

}