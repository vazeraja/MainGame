using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Rendering.UI;

#endif

namespace Aarthificial.Reanimation {
    
    [CreateAssetMenu(fileName = "ReanimatorGraph")]
    public class ResolutionGraph : ScriptableObject {
        
        public ReanimatorNode root;

        public List<ReanimatorNode> nodes = new List<ReanimatorNode>();
        
        #if UNITY_EDITOR
        public ReanimatorNode CreateNode(System.Type type)
        {
            ReanimatorNode node = ScriptableObject.CreateInstance(type) as ReanimatorNode;
            
            // ReSharper disable once PossibleNullReferenceException
            node.name = type.Name + nodes.Count;
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
            OverrideNode overrideNode = parent as OverrideNode;
            if (overrideNode) {
                overrideNode.next = child;
            }
            SwitchNode switchNode = parent as SwitchNode;
            if (switchNode) {
                switchNode.nodes.Add(child);
            }
        }
        public void RemoveChild(ReanimatorNode parent, ReanimatorNode child)
        {
            OverrideNode overrideNode = parent as OverrideNode;
            if (overrideNode) {
                overrideNode.next = null;
            }
            SwitchNode switchNode = parent as SwitchNode;
            if (switchNode) {
                switchNode.nodes.Remove(child);
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
                return switchNode.nodes;
            }
            
            SimpleAnimationNode simpleAnimation = parent as SimpleAnimationNode;
            if (simpleAnimation) {
                return children;
            }
            
            MirroredAnimationNode MirroredAnimationNode = parent as MirroredAnimationNode;
            if (MirroredAnimationNode) {
                return children;
            }

            return children;
        }
            
            
        #endif
        
        
    }
}