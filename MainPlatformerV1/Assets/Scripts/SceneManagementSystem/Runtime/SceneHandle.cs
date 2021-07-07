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
        public int sceneTag;

        public SceneHandle sceneHandle;
        
        public int sceneHandleTags;
    }

    [CreateAssetMenu(fileName = "SceneHandle", menuName = "World Graph/Scene Handle")]
    public class SceneHandle : ScriptableObject {

        public SceneAsset scene;
        
        public string[] sceneTags;
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