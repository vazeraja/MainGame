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

        // protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        // {
        //     SaveCommentBlocks();
        // }
        //
        // protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        // {
        //     SaveCommentBlocks();
        // }
        //
        // protected override void OnGroupRenamed(string oldName, string newName)
        // {
        //     SaveCommentBlocks();
        // }
        //
        // private void SaveCommentBlocks()
        // {
        //     var graphSaveData = ScriptableObject.CreateInstance<GraphSaveData>();
        //
        //     foreach (var block in CommentBlocks) {
        //         var childNodes = block.containedElements.Where(x => x is ReanimatorGraphNode)
        //             .Cast<ReanimatorGraphNode>()
        //             .Select(x => x.node.guid)
        //             .ToList();
        //         graphSaveData.groupBlocks.Add(new GroupBlock {
        //             ChildNodes = childNodes,
        //             Title = block.title,
        //             Position = block.GetPosition().position
        //         });
        //     }
        //
        //     if (graphView.graph.graphSaveData == null) {
        //         graphView.graph.CreateGraphSaveData(graphSaveData);
        //     }
        //     else {
        //         graphView.graph.graphSaveData.groupBlocks = graphSaveData.groupBlocks;
        //         
        //     }
        //
        //     Debug.Log("Saving Comment Blocks");
        // }
    }
}