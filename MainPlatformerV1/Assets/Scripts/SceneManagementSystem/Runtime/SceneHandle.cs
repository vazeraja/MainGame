#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderNut.SceneManagement {
    
    [Serializable] //needed to make ScriptableObject out of this class
    public class PassageElement
    {
        // You would only store an index to the according character
        // Since I don't have your Characters type for now lets reference them via the Dialogue.CharactersList
        public int CharacterID;

        public SceneHandle sceneHandle;
        
        // By using the attribute [TextArea] this creates a nice multi-line text are field
        // You could further configure it with a min and max line size if you want: [TextArea(minLines, maxLines)]
        [TextArea] public string DialogueText;
    }

    [CreateAssetMenu(fileName = "SceneHandle", menuName = "World Graph/Scene Handle")]
    public class SceneHandle : ScriptableObject {

        public SceneAsset scene;
        
        public string[] CharactersList;
        public List<PassageElement> passageElements;

        public void AddPassageData() {
            ScenePassage data = CreateInstance<ScenePassage>();

            AssetDatabase.AddObjectToAsset(data, this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void DeletePassage(ScenePassage data) {
            AssetDatabase.RemoveObjectFromAsset(data);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}