using System.Collections.Generic;
using System;
using Aarthificial.Reanimation.Nodes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aarthificial.Reanimation.ResolutionGraph {
    
    [CreateAssetMenu(fileName = "ReanimatorGraph")]
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
            
            Undo.RecordObject(this, "Resolution Tree (CreateNode)");
            nodes.Add(node);
            
            if (!Application.isPlaying) {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            
            Undo.RegisterCreatedObjectUndo(node, "Resolution Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(ReanimatorNode node)
        {
            Undo.RecordObject(this, "Resolution Tree (DeleteNode)");
            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }
        
        
        public void AddChild(ReanimatorNode parent, ReanimatorNode child)
        {
            if (parent is OverrideNode overrideNode) {
                Undo.RecordObject(overrideNode, "Resolution Tree (AddChild)");
                overrideNode.next = child;
                EditorUtility.SetDirty(overrideNode);
            }
            if (parent is GraphRootNode rootNode) {
                Undo.RecordObject(rootNode, "Resolution Tree (AddChild)");
                root = child;
                rootNode.root = child;
                EditorUtility.SetDirty(rootNode);
            }

            if (parent is SwitchNode switchNode) {
                Undo.RecordObject(switchNode, "Resolution Tree (AddChild)");
                switchNode.nodes.Add(child);
                EditorUtility.SetDirty(switchNode);
            }
        }
        public void RemoveChild(ReanimatorNode parent, ReanimatorNode child)
        {
            if (parent is OverrideNode overrideNode) {
                Undo.RecordObject(overrideNode, "Resolution Tree (AddChild)");
                overrideNode.next = null;
                EditorUtility.SetDirty(overrideNode);
            }

            if (parent is GraphRootNode rootNode) {
                Undo.RecordObject(rootNode, "Resolution Tree (AddChild)");
                root = null;
                rootNode.root = null;
                EditorUtility.SetDirty(rootNode);
            }
            if (parent is SwitchNode switchNode) {
                Undo.RecordObject(switchNode, "Resolution Tree (AddChild)");
                switchNode.nodes.Remove(child);
                EditorUtility.SetDirty(switchNode);
            }
        }
        #endif
        public List<ReanimatorNode> GetChildren(ReanimatorNode parent)
        {
            List<ReanimatorNode> children = new List<ReanimatorNode>();

            if (parent is GraphRootNode rootNode && root != null) {
                children.Add(root);
            }
            if (parent is OverrideNode overrideNode && overrideNode.next != null) {
                children.Add(overrideNode.next);
            }
            if (parent is SwitchNode switchNode) {
                return switchNode.nodes;
            }
            return children;
        }
        
    }
}