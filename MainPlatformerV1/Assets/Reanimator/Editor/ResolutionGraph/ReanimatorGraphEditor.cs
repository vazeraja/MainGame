using System;
using System.Collections.Generic;
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

        private void OnEnable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
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
            
            inspectorCustomControl.contentContainer.Add(new ScrollView());

            graphView.OnNodeSelected = OnNodeSelectionChanged;

            graphView.RegisterCallback<MouseDownEvent>(evt => { });
            graphView.RegisterCallback<DragExitedEvent>(evt => {
                List<ReanimatorNode> objs = new List<ReanimatorNode>();
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            
                if (DragAndDrop.objectReferences == null) return;
            
                var references = DragAndDrop.objectReferences;

                foreach (var reference in references) {
                    if (reference is ReanimatorNode scriptableObject) {
                        objs.Add(scriptableObject);
                    }
                    else {
                        EditorUtility.DisplayDialog("Invalid", "Use a Reanimator Node", "OK");
                        break;
                    }
                }
                
                Vector2 nodePosition = graphView.ChangeCoordinatesTo(graphView.contentViewContainer, evt.localMousePosition);
                objs.ForEach(node => graphView.CreateNode(node.GetType(), nodePosition ));
            });
            

            if (graph == null) {
                OnSelectionChange();
            } else {
                SelectTree(graph);
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