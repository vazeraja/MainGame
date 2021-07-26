using System;
using System.Collections.Generic;
using Aarthificial.Reanimation.ResolutionGraph.Editor;
using MainGame.DialogueGraph;
using UnityEngine;

namespace Aarthificial.Reanimation.ResolutionGraph {
    
    public class GraphSaveData : ScriptableObject {
        public List<GroupBlock> groupBlocks = new List<GroupBlock>();
    }
}