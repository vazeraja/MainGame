using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void AddChild(ReanimatorNode parent, ReanimatorNode child)
        {
            SwitchNode switchNode = parent as SwitchNode;
            if (switchNode) {
                switchNode.nodes.Add(child);
            }
            OverrideNode overrideNode = parent as OverrideNode;
            if (overrideNode) {
                overrideNode.next = child;
            }
        }
        public void RemoveChild(ReanimatorNode parent, ReanimatorNode child)
        {
            SwitchNode switchNode = parent as SwitchNode;
            if (switchNode) {
                switchNode.nodes.Remove(child);
            }
            OverrideNode overrideNode = parent as OverrideNode;
            if (overrideNode) {
                overrideNode.next = null;
            }
        }
        public List<ReanimatorNode> GetChildren(ReanimatorNode parent)
        {
            List<ReanimatorNode> children = new List<ReanimatorNode>();

            OverrideNode overrideNode = parent as OverrideNode;
            if (overrideNode && overrideNode.next != null) {
                children.Add(overrideNode.next);
            }
            
            SwitchNode switchNode = parent as SwitchNode;
            if (switchNode) {
                return switchNode.nodes.ToList();
            }

            return children;
        }
            
            
        #endif
        
        
    }
}