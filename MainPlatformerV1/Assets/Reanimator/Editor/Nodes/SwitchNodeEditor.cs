using Aarthificial.Reanimation.Nodes;
using UnityEditor;

namespace Aarthificial.Reanimation.Editor.Nodes
{
    [CustomEditor(typeof(SwitchNode))]
    public class SwitchNodeEditor : ReanimatorNodeEditor
    {
        protected void OnEnable()
        {
            AddCustomProperty("nodeTitle");
            AddCustomProperty("controlDriver");
            AddCustomProperty("drivers");
            AddCustomProperty("nodes");
        }
    }
}