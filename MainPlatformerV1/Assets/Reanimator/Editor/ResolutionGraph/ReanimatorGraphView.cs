using System;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Cels;
using Aarthificial.Reanimation.Common;
using Aarthificial.Reanimation.Nodes;
using TN.Extensions;
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
            
            AddSearchWindow(editorWindow);
            LoadGraph();
        }

        public void SaveToGraphSaveData()
        {
            var saveData = new SaveData();

            foreach (var block in CommentBlocks) {
                var childNodes = block.containedElements.Where(x => x is ReanimatorGraphNode)
                    .Cast<ReanimatorGraphNode>()
                    .Select(x => x.node.guid)
                    .ToList();

                saveData.CommentBlockData.Add(new GroupBlock() {
                    ChildNodes = childNodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }

            if (graph.SaveData == null) {
                graph.SaveData = new SaveData();
                EditorUtility.SetDirty(graph);
            }
            else {
                graph.SaveData.CommentBlockData = saveData.CommentBlockData;
                EditorUtility.SetDirty(graph);
            }
        }

        private void LoadGraph()
        {
            // Create root node if graph is empty
            if (graph.nodes.Count == 0) {
                graph.root = graph.CreateSubAsset(typeof(GraphRootNode)) as GraphRootNode;
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();
            }

            // Create every graph node from the nodes in the graph
            graph.nodes.ForEach(CreateGraphNode);

            // Create all connections based on the children of the nodes in the graph
            graph.nodes.ForEach(p => {
                var children = graph.GetChildren(p);
                children.ForEach(c => {
                    // Returns node by its guid and cast it back to a ReanimatorGraphNode
                    var parent = GetNodeByGuid(p.guid) as ReanimatorGraphNode;
                    var child = GetNodeByGuid(c.guid) as ReanimatorGraphNode;

                    if (parent?.node is GraphRootNode node && child?.node == null)
                        return;

                    var edge = parent?.output.ConnectTo(child?.input);
                    AddElement(edge);
                });
            });
            
            // Load all comment blocks and contained nodes
            foreach (var commentBlockData in graph.SaveData.CommentBlockData) {
                var block = CreateCommentBlock(new Rect(commentBlockData.Position, BlockSize),
                    commentBlockData);
                block.AddElements(GraphNodes.Where(x => commentBlockData.ChildNodes.Contains(x.node.guid)));
                
            }
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


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) => ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

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
            });

            return graphViewChange;
        }

        public void CreateNode(Type type, Vector2 nodePosition)
        {
            var node = graph.CreateSubAsset(type);
            node.position = nodePosition;
            CreateGraphNode(node);
        }

        public void CreateSimpleAnimationNode(
            ReanimatorNode node,
            IEnumerable<SimpleCel> simpleCels,
            ControlDriver controlDriver,
            DriverDictionary driverDictionary)
        {
            if (!(graph.CreateSubAsset(node.GetType()) is SimpleAnimationNode simpleAnimationNode)) return;
            var nodeSprites = simpleCels as SimpleCel[] ?? simpleCels.ToArray();
            simpleAnimationNode.sprites = nodeSprites;
            simpleAnimationNode.ControlDriver = controlDriver;
            simpleAnimationNode.Drivers = driverDictionary;

            CreateGraphNode(simpleAnimationNode);
        }

        private void CreateGraphNode(ReanimatorNode node)
        {
            var graphNode = new ReanimatorGraphNode(node) {
                OnNodeSelected = OnNodeSelected
            };

            if (node is GraphRootNode rootNode) {
                graphNode.capabilities &= ~Capabilities.Movable;
                graphNode.capabilities &= ~Capabilities.Deletable;
            }

            graphNode.OnSelected();
            AddElement(graphNode);
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
    }
}