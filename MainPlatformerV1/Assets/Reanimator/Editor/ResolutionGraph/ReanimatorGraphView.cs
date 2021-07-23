using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public static class ReanimatorNodeTypes {
    public const string SimpleAnimationNode = "SimpleAnimationNode";
    public const string SwitchNode = "SwitchNode";
    public const string OverrideNode = "OverrideNode";
    public const string MirroredAnimationNode = "MirroredAnimationNode";
}

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

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
        styleSheets.Add(styleSheet);
    }

    ReanimatorNodeDisplay FindNodeByGuid(ReanimatorNode node) => GetNodeByGuid(node.guid) as ReanimatorNodeDisplay;

    public void PopulateView(ResolutionGraph graph)
    {
        this.graph = graph;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        
        graph.nodes.ForEach(CreateNodeDisplay);
        
        graph.nodes.ForEach(n => {
            var children = graph.GetChildren(n);
            children.ForEach(c => {
                var parent = FindNodeByGuid(n);
                var child = FindNodeByGuid(n);

                var edge = parent.output.ConnectTo(child.input);
                AddElement(edge);
            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
        ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        graphViewChange.elementsToRemove?.ForEach(e => {
            if (e is ReanimatorNodeDisplay nodeDisplay) {
                graph.DeleteNode(nodeDisplay.node);
            }
        });

        if (graphViewChange.edgesToCreate != null) {
            graphViewChange.edgesToCreate.ForEach(edge => {
                ReanimatorNodeDisplay parent = edge.output.node as ReanimatorNodeDisplay;
                ReanimatorNodeDisplay child = edge.input.node as ReanimatorNodeDisplay;
                graph.AddChild(parent?.node, child?.node);
            });
        }
        
        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        {
            var types = TypeCache.GetTypesDerivedFrom<ReanimatorNode>();

            foreach (var type in types.Where(type =>
                type.Name == ReanimatorNodeTypes.SimpleAnimationNode ||
                type.Name == ReanimatorNodeTypes.SwitchNode ||
                type.Name == ReanimatorNodeTypes.OverrideNode ||
                type.Name == ReanimatorNodeTypes.MirroredAnimationNode)) {
                evt.menu.AppendAction($"{type.Name}", (a) => {
                    CreateNode(type);
                });
            }
        }
    }

    private void CreateNode(System.Type type)
    {
        ReanimatorNode node = graph.CreateNode(type);
        CreateNodeDisplay(node);
    }

    private void CreateNodeDisplay(ReanimatorNode node)
    {
        ReanimatorNodeDisplay nodeDisplay = new ReanimatorNodeDisplay(node);
        AddElement(nodeDisplay);
    }
}