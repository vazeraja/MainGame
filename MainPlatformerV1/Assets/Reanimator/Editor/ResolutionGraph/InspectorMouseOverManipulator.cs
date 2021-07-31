using Aarthificial.Reanimation.Editor.Nodes;
using Aarthificial.Reanimation.Nodes;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Object;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    
    public class InspectorMouseOverManipulator : MouseManipulator {
        
        private readonly InspectorCustomControl inspector;

        private UnityEditor.Editor editor;
        private ReanimatorNodeEditor anotherEditor;
        private AnimationNodeEditor animationEditor;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(ShowInInspector);
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(ShowInInspector);
        }

        public InspectorMouseOverManipulator(InspectorCustomControl inspector)
        {
            this.inspector = inspector;
        }
        private void ShowInInspector(MouseDownEvent evt)
        {
            // if (!(target is ReanimatorGraphNode graphNode)) return;
            
            if (!CanStopManipulation(evt))
                return;

            if (!(evt.target is ReanimatorGraphNode clickedElement)) {
                var ve = evt.target as VisualElement;
                clickedElement = ve?.GetFirstAncestorOfType<ReanimatorGraphNode>();
                if (clickedElement == null)
                    return;
            }
            
            inspector.Clear();
            
            DestroyImmediate(editor);
            DestroyImmediate(anotherEditor);
            DestroyImmediate(animationEditor);
            
            editor = UnityEditor.Editor.CreateEditor(clickedElement.node);
            anotherEditor = UnityEditor.Editor.CreateEditor(clickedElement.node) as ReanimatorNodeEditor;
            animationEditor = UnityEditor.Editor.CreateEditor(clickedElement.node) as AnimationNodeEditor;


            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    switch (clickedElement.node) {
                        case OverrideNode _:
                        case BaseNode _:
                            editor.OnInspectorGUI();
                            break;
                        case SimpleAnimationNode _:
                            animationEditor.OnInspectorGUI();
                            animationEditor.RequiresConstantRepaint();
                            animationEditor.HasPreviewGUI();
                            animationEditor.OnPreviewGUI(GUILayoutUtility.GetRect(200, 200), new GUIStyle());
                            break;
                        case SwitchNode _: {
                            anotherEditor.OnInspectorGUI();
                            break;
                        }
                    }
                }
            });
            inspector.Add(container);
        }
    }
}