using System;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using TheKiwiCoder;
using TN.Extensions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor.ResolutionGraph {
    public class ReanimatorGraphEditor : EditorWindow {
        [MenuItem("Reanimator/Resolution Graph")]
        public static void ShowWindow()
        {
            ReanimatorGraphEditor wnd = GetWindow<ReanimatorGraphEditor>();
            wnd.titleContent = new GUIContent("ReanimatorGraph");
            wnd.minSize = new Vector2(1200, 500);
        }
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line) {
            if (Selection.activeObject is Reanimation.ResolutionGraph) {
                ShowWindow();
                return true;
            }
            return false;
        }

        private VisualElement root;
        private Reanimation.ResolutionGraph graph;
        private ReanimatorGraphView graphView;
        private InspectorView inspectorView;

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
        private void OnInspectorUpdate() {
            graphView?.UpdateNodeStates();        
        }
        
        public void CreateGUI()
        {
            root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
            root.styleSheets.Add(styleSheet);

            graphView = root.Q<ReanimatorGraphView>();
            inspectorView = root.Q<InspectorView>();


            // root.RegisterCallback<MouseDownEvent>(evt => { });
            //
            // root.RegisterCallback<DragExitedEvent>(evt => {
            //     List<ScriptableObject> objs = new List<ScriptableObject>();
            //     DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            //
            //     if (DragAndDrop.objectReferences == null) return;
            //
            //     var refs = DragAndDrop.objectReferences;
            //     refs.ForEach(obj => {
            //         if (obj is ScriptableObject scriptableObject)
            //             objs.Add(scriptableObject);
            //     });
            //
            //     objs.ForEach(x => Debug.Log(x.GetType().Name));
            // });

            if (graph == null) {
                OnSelectionChange();
            } else {
                SelectTree(graph);
            }
        }

        private void OnSelectionChange()
        {
            EditorApplication.delayCall += () => {
                Reanimation.ResolutionGraph graph = Selection.activeObject as Reanimation.ResolutionGraph;
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
        private void SelectTree(Reanimation.ResolutionGraph newGraph) {

            if (graphView == null) {
                return;
            }

            if (!newGraph) {
                return;
            }

            this.graph = newGraph;

            if (Application.isPlaying) {
                graphView.PopulateView(graph);
            }
            else {
                graphView.PopulateView(graph);
            }

            EditorApplication.delayCall += () => {
                graphView.FrameAll();
            };
        }
    }
}