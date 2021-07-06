using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ThunderNut.SceneManagement.Editor {
    
    [CustomEditor(typeof(SceneHandle))]
    public class SceneHandleEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
        
        


        [MenuItem("Assets/Create/World Graph/Scene Handle (From Scene)", false, 400)]
        private static void CreateFromTextures()
        {
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
        private static bool CreateFromTexturesValidation() => Selection.activeObject as SceneAsset;
    }
}