using Aarthificial.Reanimation.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Aarthificial.Reanimation.Editor.ResolutionGraph {
    public class ReanimatorNodeDisplay : UnityEditor.Experimental.GraphView.Node {
        public ReanimatorNode node;

        public Port input;
        public Port output;

        public ReanimatorNodeDisplay() { }

        public ReanimatorNodeDisplay(ReanimatorNode node)
        {
            this.node = node;
            this.node.name = node.GetType().Name;
            title = node.name.Replace("(Clone)", "").Replace("Node", "");
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            // CreateInputPorts();
            // CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            if (node is SimpleAnimationNode) {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }
            else if (node is SwitchNode) {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }
            else if (node is OverrideNode) {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }

            if (input != null) {
                input.portName = "";
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