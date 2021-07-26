﻿using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation.Nodes;
using TN.Extensions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Aarthificial.Reanimation.ResolutionGraph.Editor {
    public sealed class ReanimatorGroup : Group {
        private GroupBlock block;
        private readonly ReanimatorGraphView graphView;

        public ReanimatorGroup(ReanimatorGraphView graphView, GroupBlock block)
        {
            this.block = block;
            this.graphView = graphView;
            autoUpdateGeometry = true;
            title = block.Title;
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            graphView.SaveToGraphSaveData();
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            graphView.SaveToGraphSaveData();
        }
    }
}