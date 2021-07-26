using System;
using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public sealed class ReanimatorGraphNode : Node {
        
        public Action<ReanimatorGraphNode> OnNodeSelected;
        public readonly ReanimatorNode node;

        public Port input;
        public Port output;

        public ReanimatorGraphNode(ReanimatorNode node)
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
                    input = new NodePort(Direction.Input, Port.Capacity.Single);
                    input.portName = "";
                    break;
                case SwitchNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single);
                    input.portName = "";
                    break;
                case OverrideNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single);
                    input.portName = "";
                    break;
                case GraphRootNode _:
                    break;
            }

            if (input != null) {
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            switch (node) {
                case SimpleAnimationNode _:
                    break;
                case SwitchNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Multi);
                    break;
                case OverrideNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Single);
                    break;
                case GraphRootNode _:
                    output = new NodePort(Direction.Output, Port.Capacity.Single);
                    break;
            }

            if (output != null) {
                output.portName = "";
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