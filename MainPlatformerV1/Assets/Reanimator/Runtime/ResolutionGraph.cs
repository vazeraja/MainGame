using System.Collections.Generic;
using Aarthificial.Reanimation.Nodes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aarthificial.Reanimation {
    
    [CreateAssetMenu(menuName = "ReanimatorGraph")]
    public class ResolutionGraph : ScriptableObject {
        
        public ReanimatorNode root;

        public List<ReanimatorNode> nodes = new List<ReanimatorNode>();
        
        #if UNITY_EDITOR
        public ReanimatorNode CreateNode(System.Type type)
        {
            ReanimatorNode node = ScriptableObject.CreateInstance(type) as ReanimatorNode;
            
            // ReSharper disable once PossibleNullReferenceException
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(ReanimatorNode node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
            
            
        #endif
        
        
    }
}