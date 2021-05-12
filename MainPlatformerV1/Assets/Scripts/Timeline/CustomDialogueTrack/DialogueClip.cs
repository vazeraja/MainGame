using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace MainGame.Timeline.CustomDialogueTrack {
    
    public class DialogueClip : PlayableAsset, ITimelineClipAsset {

        public string subtitleText;
        public Color color;
        public bool hasToPause;

        public ClipCaps clipCaps => ClipCaps.All;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

            DialogueBehaviour dialogueBehaviour = playable.GetBehaviour();
            dialogueBehaviour.subtitleText = subtitleText;
            dialogueBehaviour.subtitleColor = color;
            dialogueBehaviour.hasToPause = hasToPause;

            return playable;
        }
    }

}

