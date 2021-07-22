using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation;
using TheKiwiCoder;
using TN.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor {
    public class ReanimatorGraphEditor : EditorWindow {
        
        [MenuItem("Reanimator/Resolution Graph")]
        public static void ShowWindow()
        {
            ReanimatorGraphEditor wnd = GetWindow<ReanimatorGraphEditor>();
            wnd.titleContent = new GUIContent("ReanimatorGraphEditor");
            wnd.minSize = new Vector2(1200, 800);
        }
        
        private ReanimatorGraphView graphView;
        private InspectorView inspectorView;

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Reanimator/Editor/EditorWindow/ReanimatorGraphEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/EditorWindow/ReanimatorGraphEditor.uss");
            root.styleSheets.Add(styleSheet);

            graphView = root.Q<ReanimatorGraphView>();
            inspectorView = root.Q<InspectorView>();


            root.RegisterCallback<MouseDownEvent>(evt => { });

            root.RegisterCallback<DragExitedEvent>(evt => {
                List<ScriptableObject> objs = new List<ScriptableObject>();
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                if (DragAndDrop.objectReferences == null) return;

                var refs = DragAndDrop.objectReferences;
                refs.ForEach(obj => {
                    if (obj is ScriptableObject scriptableObject)
                        objs.Add(scriptableObject);
                });

                objs.ForEach(x => Debug.Log(x.name));
            });
        }

        private void OnSelectionChange()
        {
            ResolutionGraph graph = Selection.activeObject as ResolutionGraph;
            if (graph) {
                graphView.PopulateView(graph);
            }
        }
    }
}