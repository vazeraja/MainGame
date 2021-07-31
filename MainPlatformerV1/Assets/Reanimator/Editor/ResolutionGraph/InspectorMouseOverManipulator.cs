using Aarthificial.Reanimation.Editor.Nodes;
using Aarthificial.Reanimation.Nodes;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Object;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    
    public class InspectorMouseOverManipulator : MouseManipulator {
        
        private readonly InspectorCustomControl inspector;
        private ReanimatorGraphView reanimatorGraphView;

        private UnityEditor.Editor editor;
        private AnimationNodeEditor animationEditor;
        private SwitchNodeEditor switchNodeEditor;
        private OverrideNodeEditor overrideNodeEditor;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(ShowInInspector);
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(ShowInInspector);
        }

        public InspectorMouseOverManipulator(ReanimatorGraphView reanimatorGraphView, InspectorCustomControl inspector)
        {
            this.inspector = inspector;
            this.reanimatorGraphView = reanimatorGraphView;
        }
        private void ShowInInspector(MouseDownEvent evt)
        {
            if (!(target is ReanimatorGraphNode graphNode)) return;
            if (!CanStopManipulation(evt)) return;

            inspector.Clear();
            
            DestroyImmediate(editor);
            DestroyImmediate(switchNodeEditor);
            DestroyImmediate(animationEditor);
            DestroyImmediate(overrideNodeEditor);
            
            editor = UnityEditor.Editor.CreateEditor(graphNode.node);
            switchNodeEditor = UnityEditor.Editor.CreateEditor(graphNode.node) as SwitchNodeEditor;
            animationEditor = UnityEditor.Editor.CreateEditor(graphNode.node) as AnimationNodeEditor;
            overrideNodeEditor = UnityEditor.Editor.CreateEditor(graphNode.node) as OverrideNodeEditor;

            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    switch (graphNode.node) {
                        case OverrideNode _ when graphNode.IsSelected(reanimatorGraphView):
                            overrideNodeEditor.OnInspectorGUI();
                            break;
                        case BaseNode _ when graphNode.IsSelected(reanimatorGraphView):
                            editor.OnInspectorGUI();
                            break;
                        case SimpleAnimationNode _ when graphNode.IsSelected(reanimatorGraphView):
                            animationEditor.OnInspectorGUI();
                            animationEditor.RequiresConstantRepaint();
                            animationEditor.HasPreviewGUI();
                            animationEditor.OnPreviewGUI(GUILayoutUtility.GetRect(200, 200), new GUIStyle());
                            break;
                        case SwitchNode _ when graphNode.IsSelected(reanimatorGraphView): 
                            switchNodeEditor.OnInspectorGUI();
                            break;
                    }
                }
            });
            inspector.Add(container);
        }
    }
}