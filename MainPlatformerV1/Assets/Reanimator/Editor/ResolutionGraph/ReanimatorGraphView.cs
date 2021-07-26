using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Cels;
using Aarthificial.Reanimation.Common;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public class ReanimatorGraphView : GraphView {
        public new class UxmlFactory : UxmlFactory<ReanimatorGraphView, UxmlTraits> { }

        public ResolutionGraph graph;
        private ReanimatorSearchWindowProvider searchWindowProvider;
        private ReanimatorGraphEditor editorWindow;

        private List<ReanimatorGraphNode> GraphNodes => nodes.ToList().Cast<ReanimatorGraphNode>().ToList();
        private List<Edge> GraphEdges => edges.ToList();
        private List<Group> CommentBlocks => graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        public Action<ReanimatorGraphNode> OnNodeSelected;

        private const string styleSheetPath = "Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss";
        public readonly Vector2 BlockSize = new Vector2(300, 200);

        public ReanimatorGraphView()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);
            styleSheets.Add(styleSheet);

            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            Undo.undoRedoPerformed += () => {
                Initialize(graph, editorWindow);
                AssetDatabase.SaveAssets();
            };
        }

        public void Initialize(ResolutionGraph graph, ReanimatorGraphEditor editorWindow)
        {
            this.graph = graph;
            this.editorWindow = editorWindow;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(GraphNodes);
            DeleteElements(GraphEdges);
            DeleteElements(CommentBlocks);
            graphViewChanged += OnGraphViewChanged;

            LoadResolutionGraph();
            AddSearchWindow(editorWindow);
        }

        #region Save

        // public void SaveGraph()
        // {
        //     var graphSaveData = ScriptableObject.CreateInstance<GraphSaveData>();
        //     if (!SaveNodes(graphSaveData)) return;
        //     SaveCommentBlocks(graphSaveData);
        //
        //     if (graph.graphSaveData == null) {
        //         graph.CreateGraphSaveData(graphSaveData);
        //     }
        //     else {
        //         graph.graphSaveData.NodeLinks = graphSaveData.NodeLinks;
        //         graph.graphSaveData.ReanimatorNodeData = graphSaveData.ReanimatorNodeData;
        //         graph.graphSaveData.groupBlocks = graphSaveData.groupBlocks;
        //         EditorUtility.SetDirty(graph);
        //     }
        //
        //     AssetDatabase.SaveAssets();
        // }
        //
        // private bool SaveNodes(GraphSaveData graphSaveData)
        // {
        //     if (!GraphEdges.Any()) return false;
        //     var connectedSockets = GraphEdges.Where(x => x.input.node != null).ToArray();
        //     foreach (var t in connectedSockets) {
        //         var outputNode = t.output.node as ReanimatorGraphNode;
        //         var inputNode = t.input.node as ReanimatorGraphNode;
        //         graphSaveData.NodeLinks.Add(new NodeLinkData {
        //             BaseNodeGUID = outputNode?.guid,
        //             TargetNodeGUID = inputNode?.guid
        //         });
        //     }
        //
        //     foreach (var node in GraphNodes.Where(node => !node.EntryPoint))
        //         graphSaveData.ReanimatorNodeData.Add(new ReanimatorNodeData {
        //             NodeGUID = node.guid,
        //             Position = node.GetPosition().position
        //         });
        //
        //     return true;
        // }
        //
        // private void SaveCommentBlocks(GraphSaveData graphSaveData)
        // {
        //     foreach (var block in CommentBlocks) {
        //         var childNodes = block.containedElements.Where(x => x is ReanimatorGraphNode)
        //             .Cast<ReanimatorGraphNode>()
        //             .Select(x => x.guid)
        //             .ToList();
        //
        //         graphSaveData.groupBlocks.Add(new GroupBlock() {
        //             ChildNodes = childNodes,
        //             Title = block.title,
        //             Position = block.GetPosition().position
        //         });
        //     }
        // }
        #endregion
        private void LoadResolutionGraph()
        {
            if (graph.nodes.Count == 0) {
                graph.root = graph.CreateSubAsset(typeof(GraphRootNode)) as GraphRootNode;
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();
            }

            // Create every graph node from the nodes in the graph
            graph.nodes.ForEach(CreateGraphNode);

            // Create all connections based on the children of the nodes in the graph
            graph.nodes.ForEach(n => {
                var children = graph.GetChildren(n);
                children.ForEach(c => {
                    // Returns node by its guid and cast it back to a ReanimatorGraphNode
                    var parent = GetNodeByGuid(n.guid) as ReanimatorGraphNode;
                    var child = GetNodeByGuid(c.guid) as ReanimatorGraphNode;

                    var edge = parent?.output.ConnectTo(child?.input);
                    AddElement(edge);
                });
            });
            

            // foreach (var commentBlockData in graph.graphSaveData.groupBlocks) {
            //     var block = CreateCommentBlock(new Rect(commentBlockData.Position, BlockSize),
            //         commentBlockData);
            //     block.AddElements(GraphNodes.Where(x => commentBlockData.ChildNodes.Contains(x.guid)));
            // }
        }

        public void AddSearchWindow(ReanimatorGraphEditor editorWindow)
        {
            searchWindowProvider = ScriptableObject.CreateInstance<ReanimatorSearchWindowProvider>();
            searchWindowProvider.Initialize(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
        }

        public ReanimatorGroup CreateCommentBlock(Rect rect, GroupBlock groupBlock = null)
        {
            groupBlock ??= new GroupBlock();
            var group = new ReanimatorGroup(this, groupBlock);

            AddElement(group);
            group.SetPosition(rect);

            return group;
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
                switch (elem) {
                    case ReanimatorGraphNode nodeDisplay:
                        graph.DeleteSubAsset(nodeDisplay.node);
                        break;
                    case Edge edge: {
                        ReanimatorGraphNode parent = edge.output.node as ReanimatorGraphNode;
                        ReanimatorGraphNode child = edge.input.node as ReanimatorGraphNode;
                        graph.RemoveChild(parent?.node, child?.node);
                        break;
                    }
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge => {
                ReanimatorGraphNode parent = edge.output.node as ReanimatorGraphNode;
                ReanimatorGraphNode child = edge.input.node as ReanimatorGraphNode;

                graph.AddChild(parent?.node, child?.node);
                //SaveGraph();
            });

            return graphViewChange;
        }

        public void CreateNode(Type type, Vector2 nodePosition)
        {
            var node = graph.CreateSubAsset(type);
            node.position = nodePosition;
            CreateGraphNode(node);
        }

        public void CreateSimpleAnimationNode(ReanimatorNode node, IEnumerable<SimpleCel> simpleCels,
            ControlDriver controlDriver, DriverDictionary driverDictionary)
        {
            if (!(graph.CreateSubAsset(node.GetType()) is SimpleAnimationNode simpleAnimationNode)) return;
            var nodeSprites = simpleCels as SimpleCel[] ?? simpleCels.ToArray();
            simpleAnimationNode.sprites = nodeSprites;
            simpleAnimationNode.ControlDriver = controlDriver;
            simpleAnimationNode.Drivers = driverDictionary;

            CreateGraphNode(simpleAnimationNode);
        }

        // public void CreateSwitchNode(Type type, List<ReanimatorNode> reanimatorNodes)
        // {
        //     if (graph.CreateSubAsset(type) is SwitchNode switchNode) {
        //         switchNode.nodes = reanimatorNodes;
        //
        //         CreateGraphNode(switchNode);
        //
        //         foreach (ReanimatorNode node in switchNode.nodes) {
        //             switch (node) {
        //                 case SwitchNode innerSwitchNode: {
        //                     var innerSwitchNodes = innerSwitchNode.nodes;
        //                     EditorApplication.delayCall += () => {
        //                         CreateSwitchNode(innerSwitchNode.GetType(), innerSwitchNodes);
        //                     };
        //                     break;
        //                 }
        //                 case SimpleAnimationNode simpleAnimationNode: {
        //                     var cels = simpleAnimationNode.sprites;
        //                     var controlDriver = simpleAnimationNode.ControlDriver;
        //                     var drivers = simpleAnimationNode.Drivers;
        //                     EditorApplication.delayCall += () => {
        //                         CreateSimpleAnimationNode(simpleAnimationNode, cels, controlDriver, drivers);
        //                     };
        //                     break;
        //                 }
        //             }
        //         }
        //     }
        // }

        // public void SaveCommentBlocks()
        // {
        //     var graphSaveData = new GraphSaveData();
        //     SaveCommentBlocks(graphSaveData);
        //     
        //     
        //     graph.graphSaveData.groupBlocks = graphSaveData.groupBlocks;
        //     Debug.Log("Saving Comment Blocks");
        // }
        //
        // private void SaveCommentBlocks(GraphSaveData graphSaveData)
        // {
        //     foreach (var block in CommentBlocks) {
        //         var childNodes = block.containedElements.Where(x => x is ReanimatorGraphNode).Cast<ReanimatorGraphNode>()
        //             .Select(x => x.node.guid)
        //             .ToList();
        //
        //         graphSaveData.groupBlocks.Add(new GroupBlock {
        //             ChildNodes = childNodes,
        //             Title = block.title,
        //             Position = block.GetPosition().position
        //         });
        //     }
        // }

        private void CreateGraphNode(ReanimatorNode node)
        {
            var graphNode = new ReanimatorGraphNode(node) {
                title = node.name,
                EntryPoint = true,
                OnNodeSelected = OnNodeSelected
            };

            if (node is GraphRootNode rootNode) {
                graphNode.capabilities &= ~Capabilities.Movable;
                graphNode.capabilities &= ~Capabilities.Deletable;
            }

            graphNode.OnSelected();
            AddElement(graphNode);
        }
    }
}