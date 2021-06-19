using UnityEditor;
using UnityEngine;

namespace MainGame {
    
    //[CustomPropertyDrawer(typeof(TransitionBase), true)]
    public class TransitionsPropertyDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var decisionProperty = property.FindPropertyRelative("decision");
            
            return EditorGUI.GetPropertyHeight(decisionProperty);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var decisionProperty = property.FindPropertyRelative("decision");
            var trueStateProperty = property.FindPropertyRelative("trueState");
            var falseStateProperty = property.FindPropertyRelative("falseState");
            var enabledProperty = property.FindPropertyRelative("enabled");
            
            position.width -= 60;
            EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
            EditorGUI.PropertyField(position, decisionProperty, label, false);
            EditorGUI.EndDisabledGroup();
            
            position.x += position.width + 25;
            EditorGUI.PropertyField(position, enabledProperty, GUIContent.none);
        }

    }
}