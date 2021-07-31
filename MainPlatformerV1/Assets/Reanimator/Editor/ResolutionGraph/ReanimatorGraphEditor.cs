using System;
using System.Collections.Generic;
using Aarthificial.Reanimation.Editor;
using Aarthificial.Reanimation.Editor.Nodes;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class ReanimatorGraphEditor : EditorWindow {
        [MenuItem("Reanimator/Resolution Graph")]
        public static void ShowWindow()
        {
            ReanimatorGraphEditor wnd = GetWindow<ReanimatorGraphEditor>();
            wnd.titleContent = new GUIContent("ReanimatorGraph");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (!(Selection.activeObject is ResolutionGraph)) return false;
            ShowWindow();
            return true;
        }

        private ResolutionGraph graph;
        private ReanimatorGraphView graphView;
        private InspectorCustomControl inspectorCustomControl;
        private UnityEditor.Editor editor;
        private ReanimatorNodeEditor anotherEditor;
        
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
            root.styleSheets.Add(styleSheet);

            graphView = root.Q<ReanimatorGraphView>();
            inspectorCustomControl = root.Q<InspectorCustomControl>();

            graphView.OnNodeSelected = OnNodeSelectionChanged;

            if (graph == null) {
                OnSelectionChange();
            }
            else {
                SelectTree(graph);
            }
        }
        
        private void OnNodeSelectionChanged(ReanimatorGraphNode node)
        {
            inspectorCustomControl.Clear();
            DestroyImmediate(editor);
            DestroyImmediate(anotherEditor);

            editor = UnityEditor.Editor.CreateEditor(node.node);
            anotherEditor = UnityEditor.Editor.CreateEditor(node.node) as ReanimatorNodeEditor;

            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    switch (node.node) {
                        case OverrideNode _:
                        case BaseNode _:
                            editor.OnInspectorGUI();
                            break;
                        case SimpleAnimationNode _:
                        case SwitchNode _: {
                            anotherEditor.OnInspectorGUI();
                            if (node.node is SimpleAnimationNode) {
                                Helpers.DrawTexturePreview(GUILayoutUtility.GetRect(150, 150), node.graph.sprite);
                            }

                            break;
                        }
                    }
                }
            });

            inspectorCustomControl.Add(container);
        }
        
        private void OnSelectionChange()
        {
            EditorApplication.delayCall += () => {
                ResolutionGraph graph = Selection.activeObject as ResolutionGraph;
                if (!graph) {
                    if (Selection.activeGameObject) {
                        Reanimator reanimator = Selection.activeGameObject.GetComponent<Reanimator>();
                        if (reanimator) {
                            graph = reanimator.graph.Value;
                        }
                    }
                }

                SelectTree(graph);
            };
        }

        private void SelectTree(ResolutionGraph newGraph)
        {
            if (graphView == null || !newGraph) {
                return;
            }
            
            graph = newGraph;

            if (Application.isPlaying) {
                graphView.Initialize(graph, this);
            }
            else {
                graphView.Initialize(graph, this);
            }

            EditorApplication.delayCall += () => { graphView.FrameAll(); };
        }
    }
}