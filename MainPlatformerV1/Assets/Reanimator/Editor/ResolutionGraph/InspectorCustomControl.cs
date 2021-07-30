using Aarthificial.Reanimation.Editor;
using Aarthificial.Reanimation.Editor.Nodes;
using Aarthificial.Reanimation.Nodes;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class InspectorCustomControl : ScrollView {
        public new class UxmlFactory : UxmlFactory<InspectorCustomControl, ScrollView.UxmlTraits> { }

        UnityEditor.Editor editor;
        AnimationNodeEditor anotherEditor;

        public void UpdateSelection(ReanimatorGraphNode graphNode)
        {
            Clear();
            Object.DestroyImmediate(editor);
            Object.DestroyImmediate(anotherEditor);
            
            editor = UnityEditor.Editor.CreateEditor(graphNode.node);
            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    editor.OnInspectorGUI();
                }

                if (graphNode.node is SimpleAnimationNode simpleAnimationNode && editor && editor.target) {
                    Helpers.DrawTexturePreview(GUILayoutUtility.GetRect(150, 150), graphNode.graph.sprite);
                }
                
            });
            Add(container);
        }
    }
}