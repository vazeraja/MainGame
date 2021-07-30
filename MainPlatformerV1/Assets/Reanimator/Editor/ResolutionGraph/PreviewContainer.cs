using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Cels;
using Aarthificial.Reanimation.Editor.Nodes;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class PreviewContainer : VisualElement {
        public new class UxmlFactory : UxmlFactory<PreviewContainer, VisualElement.UxmlTraits> { }

        public UnityEditor.Editor editor;
        
        public void DisplaySpritePreview(ReanimatorGraphNode node)
        {
            Clear();
            Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(node.graph);
                
            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    editor.OnInspectorGUI();
                    editor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
                }
                    
            });
            Add(container);
        }
    }
}