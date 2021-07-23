using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor.ResolutionGraph {
    public class ReanimatorGraphView : GraphView {
        public new class UxmlFactory : UxmlFactory<ReanimatorGraphView, UxmlTraits> { }
        
        public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
        private Reanimation.ResolutionGraph graph;
        
        public ReanimatorGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
            styleSheets.Add(styleSheet);
            
            AddElement(GetEntryPointNodeInstance());
        }

        ReanimatorNodeDisplay FindNodeByGuid(ReanimatorNode node) => GetNodeByGuid(node.guid) as ReanimatorNodeDisplay;

        public void PopulateView(Reanimation.ResolutionGraph graph)
        {
            this.graph = graph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;
            
            graph.nodes.ForEach(CreateNodeDisplay);
            
            // graph.nodes.ForEach(n => {
            //     var children = Reanimation.ResolutionGraph.GetChildren(n);
            //     children.ForEach(c => {
            //         var parent = FindNodeByGuid(n);
            //         var child = FindNodeByGuid(c);
            //         var edge = parent.output.ConnectTo(child.input);
            //         AddElement(edge);
            //         
            //     });
            // });
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
                if (elem is ReanimatorNodeDisplay nodeDisplay) {
                    graph.DeleteNode(nodeDisplay.node);
                }
                // else if (elem is Edge edge) {
                //     ReanimatorNodeDisplay parent = edge.output.node as ReanimatorNodeDisplay;
                //     ReanimatorNodeDisplay child = edge.input.node as ReanimatorNodeDisplay;
                //     Debug.Log(parent.node.name + " " + child.node.name);
                //     graph.RemoveChild(parent.node, child.node);
                // }
            });

            // graphViewChange.edgesToCreate?.ForEach(edge => {
            //     ReanimatorNodeDisplay parent = edge.output.node as ReanimatorNodeDisplay;
            //     ReanimatorNodeDisplay child = edge.input.node as ReanimatorNodeDisplay;
            //     
            //     Debug.Log(parent.node.name + " " + child.node.name);
            //     graph.AddChild(parent.node, child.node);
            // });

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            {
                evt.menu.AppendAction($"SimpleAnimationNode", (a) => { CreateSubSO(typeof(SimpleAnimationNode)); });
                evt.menu.AppendAction($"SwitchNode", (a) => { CreateSubSO(typeof(SwitchNode)); });
                evt.menu.AppendAction($"OverrideNode", (a) => { CreateSubSO(typeof(OverrideNode)); });
            }
        }

        private void CreateSubSO(Type type)
        {
            var node = graph.CreateNode(type);
            CreateNodeDisplay(node);
        }

        private void CreateNodeDisplay(ReanimatorNode node)
        {
            ReanimatorNodeDisplay nodeDisplay = new ReanimatorNodeDisplay(node);

            if (nodeDisplay.node is SimpleAnimationNode) {
                var inputPort = new NodePort(Direction.Input, Port.Capacity.Single) {
                    portName = "SimpleAnimation"
                };
                nodeDisplay.inputContainer.Add(inputPort);
                nodeDisplay.RefreshExpandedState();
                nodeDisplay.RefreshPorts();
            }
            if (nodeDisplay.node is SwitchNode) {
                var inputPort = new NodePort(Direction.Input, Port.Capacity.Single) {
                    portName = "Input"
                };
                nodeDisplay.inputContainer.Add(inputPort);
                nodeDisplay.RefreshExpandedState();
                nodeDisplay.RefreshPorts();

                var outputPort = new NodePort(Direction.Output, Port.Capacity.Multi) {
                    portName = "Switch"
                };
                nodeDisplay.outputContainer.Add(outputPort);
                nodeDisplay.RefreshExpandedState();
                nodeDisplay.RefreshPorts();
            }
            
            
            AddElement(nodeDisplay);
        }
        private ReanimatorNodeDisplay GetEntryPointNodeInstance(){
            graph.CreateNode(typeof(SwitchNode));
            
            var nodeCache = new ReanimatorNodeDisplay {
                title = "Root",
            };
            var generatedPort = new NodePort(Direction.Output, Port.Capacity.Single) {
                portName = "Next"
            };
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(100, 200, 100, 150));
            return nodeCache;
        }

        public void UpdateNodeStates()
        {
            nodes.ForEach(n => {
                ReanimatorNodeDisplay view = n as ReanimatorNodeDisplay;
                //view.UpdateState();
            });
        }
    }
}