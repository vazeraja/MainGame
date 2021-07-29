using System;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public sealed class ReanimatorGraphNode : Node {
        
        public readonly ReanimatorNode node;
        public readonly ResolutionGraph graph; 
        
        public Action<ReanimatorGraphNode> OnNodeSelected;

        public Port input;
        public Port output;

        public ReanimatorGraphNode(ReanimatorNode node, ResolutionGraph graph) : base("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphNode.uxml")
        {
            // UseDefaultStyling();
            this.graph = graph;
            this.node = node;
            
            this.node.name = node.nodeTitle == string.Empty 
                ? node.GetType().Name 
                : node.nodeTitle;
            title = node.nodeTitle == string.Empty
                ? node.name.Replace("(Clone)", "").Replace("Node", "")
                : node.nodeTitle;
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            CreateTitleEditField();
            SetupClasses();
            EditorUtility.SetDirty(this.graph);
            
            switch (node) {
                case BaseNode _:
                    capabilities &= ~Capabilities.Movable;
                    capabilities &= ~Capabilities.Deletable;
                    break;
                case SwitchNode switchNode:
                    break;
            }
        }

        private void SetupClasses()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    AddToClassList("simpleAnimation");
                    break;
                case SwitchNode _:
                    AddToClassList("switch");
                    break;
                case OverrideNode _:
                    AddToClassList("override");
                    break;
                case BaseNode _:
                    AddToClassList("base");
                    break;
            }
        }

        private void CreateTitleEditField()
        {
            var textField = new TextField();
            textField.RegisterValueChangedCallback(evt => {
                title = evt.newValue;
                node.nodeTitle = evt.newValue;
            });
            extensionContainer.Add(textField);
        }

        private void CreateInputPorts()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(SimpleAnimationNode));
                    break;
                case SwitchNode _:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(SwitchNode));
                    break;
                case OverrideNode _:
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(OverrideNode));
                    break;
                case BaseNode _:
                    break;
            }

            if (input == null) return;
            input.portName = "";
            inputContainer.Add(input);
        }

        private void CreateOutputPorts()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    break;
                case SwitchNode _:
                    output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                        typeof(SwitchNode));
                    break;
                case OverrideNode _:
                    output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                        typeof(OverrideNode));
                    break;
                case BaseNode _:
                    output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                        typeof(BaseNode));
                    break;
            }

            if (output == null) return;
            output.portName = "";
            outputContainer.Add(output);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "ResolutionGraph (Set Position)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}