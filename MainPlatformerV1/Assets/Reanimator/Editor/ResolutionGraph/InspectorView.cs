using UnityEngine.UIElements;
using UnityEditor;

namespace Aarthificial.Reanimation.Editor.ResolutionGraph {
    public class InspectorView : VisualElement {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        UnityEditor.Editor editor;

        public InspectorView() {

        }

        internal void UpdateSelection(ReanimatorNodeDisplay nodeView) {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);

            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target) {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
    }
}