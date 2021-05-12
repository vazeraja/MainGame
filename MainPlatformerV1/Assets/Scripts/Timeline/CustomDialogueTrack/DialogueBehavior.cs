using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace MainGame.Timeline.CustomDialogueTrack {
    
    public class DialogueBehaviour : PlayableBehaviour {

        [HideInInspector] public bool hasToPause = false;
        [HideInInspector] public string subtitleText;
        [HideInInspector] public Color subtitleColor;

        private bool clipPlayed = false;
        private bool pauseScheduled = false;
        
        private PlayableDirector director;
        
        public override void OnPlayableCreate(Playable playable) {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
        
            TextMeshProUGUI text = playerData as TextMeshProUGUI;
            text.text = subtitleText;
            text.color = new Color(subtitleColor.r, subtitleColor.g, subtitleColor.b, info.weight);
        
            if (!clipPlayed && info.weight > 0f) {
                if (Application.isPlaying) {
                    if (hasToPause) {
                        pauseScheduled = true;
                    }
                }
        
                clipPlayed = true;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            if (pauseScheduled) {
                pauseScheduled = false;
                director.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            }
        
            clipPlayed = false;
        }
    }
}
