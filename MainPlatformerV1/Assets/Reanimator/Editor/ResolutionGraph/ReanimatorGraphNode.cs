using System;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public sealed class ReanimatorGraphNode : Node {
        
        public readonly ReanimatorNode node;
        
        public Action<ReanimatorGraphNode> OnNodeSelected;

        public Port input;
        public Port output;

        public ReanimatorGraphNode(ReanimatorNode node) //: base("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphNode.uxml")
        {
            this.node = node;
            this.node.name = node.GetType().Name;
            title = node.name.Replace("(Clone)", "").Replace("Node", "");
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single, Orientation.Horizontal);
                    break;
                case SwitchNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single, Orientation.Horizontal);
                    break;
                case OverrideNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single, Orientation.Horizontal);
                    break;
                case GraphRootNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single, Orientation.Horizontal);
                    input.visible = false;
                    input.capabilities &= ~Capabilities.Selectable;
                    break;
            }

            if (input != null) {
                input.portName = "";
                //input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Multi, Orientation.Horizontal);
                    output.visible = false;
                    output.capabilities &= ~Capabilities.Selectable;
                    break;
                case SwitchNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Multi, Orientation.Horizontal);
                    break;
                case OverrideNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Single, Orientation.Horizontal);
                    break;
                case GraphRootNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Single, Orientation.Horizontal);
                    break;
            }

            if (output != null) {
                output.portName = "";
                //output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
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