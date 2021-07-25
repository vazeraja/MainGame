using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation;
using Aarthificial.Reanimation.Nodes;
using TheKiwiCoder;
using TN.Extensions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class ReanimatorGraphEditor : EditorWindow {
        [MenuItem("Reanimator/Resolution Graph")]
        public static void ShowWindow()
        {
            ReanimatorGraphEditor wnd = GetWindow<ReanimatorGraphEditor>();
            wnd.titleContent = new GUIContent("ReanimatorGraph");
            wnd.minSize = new Vector2(1200, 800);
        }
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line) {
            if (Selection.activeObject is ResolutionGraph) {
                ShowWindow();
                return true;
            }
            return false;
        }

        private VisualElement root;
        private ResolutionGraph graph;
        private ReanimatorGraphView graphView;
        private InspectorCustomControl inspectorCustomControl;
        
        private List<ReanimatorNode> draggedNodes = new List<ReanimatorNode>();

        private void OnEnable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnInspectorUpdate()
        {
            if (draggedNodes.Any()) {
                draggedNodes.ForEach(node => {
                    if (node is SimpleAnimationNode simpleAnimationNode) {
                        var cels = simpleAnimationNode.sprites;
                        cels.ForEach(sprite => {
                            Debug.Log(sprite.Sprite.name);
                        });
                    }
                    graphView.CreateNode(node.GetType(), new Vector2());
                    
                });
                draggedNodes.Clear();
            }
        }

        public void CreateGUI()
        {
            root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
            root.styleSheets.Add(styleSheet);

            graphView = root.Q<ReanimatorGraphView>();
            inspectorCustomControl = root.Q<InspectorCustomControl>();

            graphView.OnNodeSelected = OnNodeSelectionChanged;
            graphView.RegisterCallback<DragExitedEvent>(evt => {
                if (DragAndDrop.objectReferences == null) return;
            
                var references = DragAndDrop.objectReferences;

                foreach (var reference in references) {
                    if (reference is ReanimatorNode scriptableObject) {
                        draggedNodes.Add(scriptableObject);
                    } 
                    else {
                        EditorUtility.DisplayDialog("Invalid", "Use a Reanimator Node", "OK");
                        break;
                    }
                }
            });

            if (graph == null) {
                OnSelectionChange();
            } else {
                SelectTree(graph);
            }
        }
        private void OnPlayModeStateChanged(PlayModeStateChange obj) {
            switch (obj) {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

        private void OnNodeSelectionChanged(ReanimatorGraphNode node)
        {
            inspectorCustomControl.UpdateSelection(node);
        }

        private void OnSelectionChange()
        {
            EditorApplication.delayCall += () => {
                ResolutionGraph graph = Selection.activeObject as ResolutionGraph;
                if (!graph) {
                    if (Selection.activeGameObject) {
                        Reanimator reanimator = Selection.activeGameObject.GetComponent<Reanimator>();
                        if (reanimator) {
                            graph = reanimator.graph;
                        }
                    }
                }

                SelectTree(graph);
            };
        }
        private void SelectTree(ResolutionGraph newGraph) {

            if (graphView == null) {
                return;
            }

            if (!newGraph) {
                return;
            }

            this.graph = newGraph;

            if (Application.isPlaying) {
                graphView.PopulateView(graph, this);
            }
            else {
                graphView.PopulateView(graph, this);
            }

            EditorApplication.delayCall += () => {
                graphView.FrameAll();
            };
        }
    }
}