using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor.ResolutionGraph {
    public class ReanimatorGraphNode : UnityEditor.Experimental.GraphView.Node {
        public readonly ReanimatorNode node;

        public Port input;
        public Port output;

        public ReanimatorGraphNode() { }

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
                    input.portName = "SwitchNode";
                    break;
                case OverrideNode _:
                    input = new NodePort(Direction.Input, Port.Capacity.Single);
                    input.portName = "OverrideNode";
                    break;
            }

            if (input != null) {
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            if (node is SimpleAnimationNode) {
                
            }
            else if (node is SwitchNode) {
                output = new NodePort(Direction.Output, Port.Capacity.Multi);
            }
            else if (node is OverrideNode) {
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            }

            if (output != null) {
                output.portName = "";
                outputContainer.Add(output);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Resolution Tree (Set Position)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }
    }
}