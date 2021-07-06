using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneHandle", menuName = "World Graph/Scene Handle")]
public class SceneHandle : ScriptableObject {

    public SceneAsset scene;
    

}