using System;
using System.Collections.Generic;
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

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

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
            graphView.RegisterCallback<DragExitedEvent>(evt => { CreateDragAndDropNodes(); });
            
            if (graph == null) {
                OnSelectionChange();
            }
            else {
                SelectTree(graph);
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
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

        private void CreateDragAndDropNodes()
        {
            if (DragAndDrop.objectReferences == null) return;

            var references = DragAndDrop.objectReferences;
            List<ReanimatorNode> draggedNodes = new List<ReanimatorNode>();

            foreach (var reference in references) {
                if (reference is ReanimatorNode reanimatorNode) {
                    draggedNodes.Add(reanimatorNode);

                    foreach (ReanimatorNode node in draggedNodes) {
                        switch (node) {
                            case SimpleAnimationNode simpleAnimationNode: {
                                var cels = simpleAnimationNode.sprites;
                                var controlDriver = simpleAnimationNode.ControlDriver;
                                var drivers = simpleAnimationNode.Drivers;
                                EditorApplication.delayCall += () => {
                                    graphView.CreateSimpleAnimationNode(node, cels, controlDriver, drivers);
                                };
                                break;
                            }
                            case SwitchNode switchNode: {
                                EditorApplication.delayCall += () => {
                                    var nodes = switchNode.nodes;
                                    graphView.CreateSwitchNode(switchNode.GetType(), nodes);
                                };
                                break;
                            }
                            // case OverrideNode overrideNode: {
                            //     EditorApplication.delayCall += delegate {
                            //         graphView.CreateNode(overrideNode.GetType(), new Vector2());
                            //     };
                            //     break;
                            // }
                        }
                    }
                }
                else {
                    EditorUtility.DisplayDialog("Invalid", "You dumb cunt, use a Reanimator Node,not whatever that is", "OK");
                    break;
                }
            }
        }

        private void OnSelectionChange()
        {
            EditorApplication.delayCall += () => {
                ResolutionGraph graph = Selection.activeObject as ResolutionGraph;
                if (!graph) {
                    if (Selection.activeGameObject) {
                        Reanimator reanimator = Selection.activeGameObject.GetComponent<Reanimator>();
                        if (reanimator) {
                            //graph = reanimator.graph;
                        }
                    }
                }

                SelectTree(graph);
            };
        }

        private void SelectTree(ResolutionGraph newGraph)
        {
            if (graphView == null) {
                return;
            }

            if (!newGraph) {
                return;
            }

            this.graph = newGraph;

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