using Aarthificial.Reanimation.Nodes;
using UnityEngine.UIElements;
using UnityEditor;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class InspectorView : VisualElement {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        UnityEditor.Editor editor;

        public InspectorView()
        { }

        internal void UpdateSelection(ReanimatorGraphNode graphNode)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);

            editor = UnityEditor.Editor.CreateEditor(graphNode.node);

            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
    }
}