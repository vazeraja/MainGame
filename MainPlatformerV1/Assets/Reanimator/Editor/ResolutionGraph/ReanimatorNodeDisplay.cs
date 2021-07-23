using Aarthificial.Reanimation.Nodes;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public sealed class ReanimatorNodeDisplay : UnityEditor.Experimental.GraphView.Node {

    public ReanimatorNode node;

    public Port input;
    public Port output;
        
    public ReanimatorNodeDisplay(ReanimatorNode node)
    {
        this.node = node;
        title = node.name;
        viewDataKey = node.guid;
        
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void CreateInputPorts()
    {
        switch (node.GetType().Name) {
            case ReanimatorNodeTypes.SimpleAnimationNode:
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
                break;
            case ReanimatorNodeTypes.SwitchNode:
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
            case ReanimatorNodeTypes.OverrideNode:
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
            case ReanimatorNodeTypes.MirroredAnimationNode:
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
                break;
        }

        if (input == null) return;
        input.portName = "";
        inputContainer.Add(input);
    }
    private void CreateOutputPorts()
    {
        switch (node.GetType().Name) {
            case ReanimatorNodeTypes.SimpleAnimationNode:
                break;
            case ReanimatorNodeTypes.SwitchNode:
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                break;
            case ReanimatorNodeTypes.OverrideNode:
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
            case ReanimatorNodeTypes.MirroredAnimationNode:
                break;
        }
        
        if (output == null) return;
        output.portName = "";
        outputContainer.Add(output);
    }
    
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }
}