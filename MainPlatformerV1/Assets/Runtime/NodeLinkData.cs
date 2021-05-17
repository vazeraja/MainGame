using System;
using System.Linq;

namespace MainGame.DialogueGraph {
    
    [Serializable]
    public class NodeLinkData {
        public string BaseNodeGUID;
        public string PortName;
        public string TargetNodeGUID;
    }
}
