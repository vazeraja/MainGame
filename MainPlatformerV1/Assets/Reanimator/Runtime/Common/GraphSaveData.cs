using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aarthificial.Reanimation.ResolutionGraph {
    
    [Serializable]
    public class NodeLinkData {
        public string BaseNodeGUID;
        public string TargetNodeGUID;
    }
    [Serializable]
    public class ReanimatorNodeData {
        public string NodeGUID;
        public Vector2 Position;
    }
    [Serializable]
    public class GroupBlock {
        public List<string> ChildNodes = new List<string>();
        public Vector2 Position;
        public string Title = "Group Block";
    }
    
    public class GraphSaveData : ScriptableObject {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<ReanimatorNodeData> ReanimatorNodeData = new List<ReanimatorNodeData>();
        public List<GroupBlock> groupBlocks = new List<GroupBlock>();
    }
}