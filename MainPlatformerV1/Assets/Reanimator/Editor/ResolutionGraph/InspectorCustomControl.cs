using Aarthificial.Reanimation.Nodes;
using UnityEngine.UIElements;
using UnityEditor;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class InspectorCustomControl : ScrollView {
        public new class UxmlFactory : UxmlFactory<InspectorCustomControl, ScrollView.UxmlTraits> { }

        UnityEditor.Editor editor;

        public void UpdateSelection(ReanimatorGraphNode graphNode)
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