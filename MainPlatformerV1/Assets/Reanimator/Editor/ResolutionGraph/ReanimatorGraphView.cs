using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class ReanimatorGraphView : GraphView {
        public new class UxmlFactory : UxmlFactory<ReanimatorGraphView, UxmlTraits> { }
        
        public Action<ReanimatorGraphNode> OnNodeSelected;
        private const string styleSheetPath = "Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss";
        private ResolutionGraph graph;

        public ReanimatorGraphView()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);
            styleSheets.Add(styleSheet);
            
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(graph);
            AssetDatabase.SaveAssets();
        }

        public ReanimatorGraphNode FindNodeByGuid(ReanimatorNode node) => GetNodeByGuid(node.guid) as ReanimatorGraphNode;

        public void PopulateView(ResolutionGraph graph)
        {
            this.graph = graph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;
            
            if (graph.nodes.Count == 0) {
                graph.root = graph.CreateNode(typeof(GraphRootNode)) as GraphRootNode;
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();
            }
                        
            graph.nodes.ForEach(CreateGraphNode);

            graph.nodes.ForEach(n => {
                var children = graph.GetChildren(n);
                children.ForEach(c => {
                    var parent = FindNodeByGuid(n);
                    var child = FindNodeByGuid(c);

                    var rootGraphNode = parent.node as GraphRootNode;
                    if (!rootGraphNode) {
                        var edge = parent.output.ConnectTo(child.input);
                        AddElement(edge);
                    }
                });
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove?.ForEach(elem => {
                if (elem is ReanimatorGraphNode nodeDisplay) {
                    graph.DeleteNode(nodeDisplay.node);
                }
                else if (elem is Edge edge) {
                    ReanimatorGraphNode parent = edge.output.node as ReanimatorGraphNode;
                    ReanimatorGraphNode child = edge.input.node as ReanimatorGraphNode;
                    graph.RemoveChild(parent?.node, child?.node);
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge => {
                ReanimatorGraphNode parent = edge.output.node as ReanimatorGraphNode;
                ReanimatorGraphNode child = edge.input.node as ReanimatorGraphNode;

                graph.AddChild(parent?.node, child?.node);
            });

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            {
                evt.menu.AppendAction($"[Reanimator]/SimpleAnimationNode", (a) => {
                    var node = graph.CreateNode(typeof(SimpleAnimationNode));
                    node.position = nodePosition;
                    CreateGraphNode(node);
                });
                evt.menu.AppendAction($"[Reanimator]/SwitchNode", (a) => {
                    var node = graph.CreateNode(typeof(SwitchNode));
                    node.position = nodePosition;
                    CreateGraphNode(node);
                });
                evt.menu.AppendAction($"[Reanimator]/OverrideNode", (a) => {
                    var node = graph.CreateNode(typeof(OverrideNode));
                    node.position = nodePosition;
                    CreateGraphNode(node);
                });
                
            }
        }

        private void CreateGraphNode(ReanimatorNode node)
        {
            var rootGraphNode = node as GraphRootNode;

            var graphNode = new ReanimatorGraphNode(node) {
                OnNodeSelected = OnNodeSelected
            };
            
            if (rootGraphNode) {
                graphNode.capabilities &= ~Capabilities.Movable;
                graphNode.capabilities &= ~Capabilities.Deletable;
            }
            
            graphNode.OnSelected();
            AddElement(graphNode);
        }
    }
}