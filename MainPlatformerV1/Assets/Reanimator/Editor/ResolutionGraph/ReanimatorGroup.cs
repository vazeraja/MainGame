using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public sealed class ReanimatorGroup : Group {
        private GroupBlock block;
        private ReanimatorGraphView graphView;

        public List<Group> CommentBlocks =>
            graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        public ReanimatorGroup(ReanimatorGraphView graphView, GroupBlock block)
        {
            this.block = block;
            this.graphView = graphView;
            autoUpdateGeometry = true;
            title = block.Title;
        }

    }
}