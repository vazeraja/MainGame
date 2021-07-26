﻿using System.Collections.Generic;
using System;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using Aarthificial.Reanimation.ResolutionGraph.Editor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aarthificial.Reanimation.ResolutionGraph {
    
    [Serializable]
    public class SaveData {
        public List<GroupBlock> CommentBlockData = new List<GroupBlock>();
    }
    
    [CreateAssetMenu(fileName = "ResolutionGraph", menuName = "Reanimator/ResolutionGraph", order = 400)]
    public class ResolutionGraph : ScriptableObject {
        public ReanimatorNode root;

        public List<ReanimatorNode> nodes = new List<ReanimatorNode>();
        private List<ReanimatorNode> currentTrace = new List<ReanimatorNode>();

        public SaveData SaveData = new SaveData();

        
        #region Editor
        #if UNITY_EDITOR
        public ReanimatorNode CreateSubAsset(Type type, string assetName = null)
        {
            ReanimatorNode node = ScriptableObject.CreateInstance(type) as ReanimatorNode;

            // ReSharper disable once PossibleNullReferenceException
            node.name = string.IsNullOrEmpty(assetName) ? type.Name : assetName;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Resolution Tree");
            nodes.Add(node);
            if (!Application.isPlaying) {
                AssetDatabase.AddObjectToAsset(node, this);
            }

            Undo.RegisterCreatedObjectUndo(node, "Resolution Tree");

            AssetDatabase.SaveAssets();
            return node;
        }
        
        public void DeleteSubAsset(ReanimatorNode node)
        {
            Undo.RecordObject(this, "Resolution Tree");
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }


        public void AddChild(ReanimatorNode parent, ReanimatorNode child)
        {
            switch (parent) {
                case GraphRootNode rootNode:
                    Undo.RecordObject(rootNode, "Resolution Tree");
                    root = child;
                    rootNode.root = child;
                    EditorUtility.SetDirty(rootNode);
                    break;
                case OverrideNode overrideNode:
                    Undo.RecordObject(overrideNode, "Resolution Tree");
                    overrideNode.next = child;
                    EditorUtility.SetDirty(overrideNode);
                    break;
                case SwitchNode switchNode:
                    Undo.RecordObject(switchNode, "Resolution Tree");
                    switchNode.nodes.Add(child);
                    EditorUtility.SetDirty(switchNode);
                    break;
            }
        }

        public void RemoveChild(ReanimatorNode parent, ReanimatorNode child)
        {
            switch (parent) {
                case GraphRootNode rootNode:
                    Undo.RecordObject(rootNode, "Resolution Tree");
                    root = null;
                    rootNode.root = null;
                    EditorUtility.SetDirty(rootNode);
                    break;
                case OverrideNode overrideNode:
                    Undo.RecordObject(overrideNode, "Resolution Tree");
                    overrideNode.next = null;
                    EditorUtility.SetDirty(overrideNode);
                    break;
                case SwitchNode switchNode:
                    Undo.RecordObject(switchNode, "Resolution Tree");
                    switchNode.nodes.Remove(child);
                    EditorUtility.SetDirty(switchNode);
                    break;
            }
        }
        #endif
        #endregion
        
        public List<ReanimatorNode> GetChildren(ReanimatorNode parent)
        {
            List<ReanimatorNode> children = new List<ReanimatorNode>();

            switch (parent) {
                case GraphRootNode rootNode when root != null:
                    children.Add(root);
                    break;
                case OverrideNode overrideNode when overrideNode.next != null:
                    children.Add(overrideNode.next);
                    break;
                case SwitchNode switchNode:
                    return switchNode.nodes;
            }

            return children;
        }

        public ResolutionGraph GetCopy()
        {
            ResolutionGraph graph = Instantiate(this);
            graph.root = graph.root.Copy();
            return graph;
        }
    }
}