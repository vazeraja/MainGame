#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

namespace ThunderNut.SceneManagement {
    
    [Serializable]
    public class PassageElement
    {
        public int sceneTag;
        public SceneHandle sceneHandle;
        public int sceneHandleTags;
    }

    [CreateAssetMenu(fileName = "SceneHandle", menuName = "World Graph/Scene Handle")]
    public class SceneHandle : ScriptableObject {

        public SceneAsset scene;
        public List<PassageElement> passageElements;
        [HideInInspector] public string[] sceneTags;
        
        [HideInInspector] public List<ScenePassage> scenePassages;

        public void AddPassage(SceneHandle sceneHandle) {
            ScenePassage data = CreateInstance<ScenePassage>();
            
            scenePassages.Add(data);
            
            AssetDatabase.AddObjectToAsset(data, sceneHandle);
            EditorUtility.SetDirty(sceneHandle);
            AssetDatabase.SaveAssets();
        }

        public void DeletePassage(ScenePassage data) {

            AssetDatabase.RemoveObjectFromAsset(data);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}