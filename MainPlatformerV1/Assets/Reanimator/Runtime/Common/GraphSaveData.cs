using System;
using System.Collections.Generic;
using Aarthificial.Reanimation.ResolutionGraph.Editor;
using MainGame.DialogueGraph;
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
    
    public class GraphSaveData : ScriptableObject {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<ReanimatorNodeData> ReanimatorNodeData = new List<ReanimatorNodeData>();
        public List<GroupBlock> groupBlocks = new List<GroupBlock>();
    }
}