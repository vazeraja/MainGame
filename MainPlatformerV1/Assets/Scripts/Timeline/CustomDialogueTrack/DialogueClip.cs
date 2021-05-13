using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace MainGame.Timeline.CustomDialogueTrack {

    public class DialogueClip : PlayableAsset, ITimelineClipAsset {

        // public string subtitleText;
        // public Color color;
        public DialogueObject dialogue;
        public bool hasToPause;
        public int index;
        public DialogueBehaviour.DialogueType dialogueType;
        public InputReader inputReader;

        public ClipCaps clipCaps => ClipCaps.All;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

            DialogueBehaviour dialogueBehaviour = playable.GetBehaviour();
            dialogueBehaviour.dialogue = dialogue;
            dialogueBehaviour.hasToPause = hasToPause;
            dialogueBehaviour.index = index;
            dialogueBehaviour.dialogueType = dialogueType;
            dialogueBehaviour.inputReader = inputReader;

            return playable;
        }
    }

}

