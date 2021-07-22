using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor {
    public class ReanimatorGraphView : GraphView {
        public new class UxmlFactory : UxmlFactory<ReanimatorGraphView, UxmlTraits> { }

        private ResolutionGraph graph;

        public ReanimatorGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/EditorWindow/ReanimatorGraphEditor.uss");
            styleSheets.Add(styleSheet);
        }

        public void PopulateView(ResolutionGraph graph)
        {
            this.graph = graph;

            DeleteElements(graphElements);

            graph.nodes.ForEach(CreateNodeView);
        }

        private void CreateNodeView(ReanimatorNode node)
        {
            ReanimatorNodeDisplay nodeDisplay = new ReanimatorNodeDisplay(node);
            AddElement(nodeDisplay);
        }
    }
}