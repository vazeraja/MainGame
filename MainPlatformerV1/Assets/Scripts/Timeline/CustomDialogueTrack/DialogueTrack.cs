using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MainGame.Timeline.CustomDialogueTrack {
    
    [TrackColor(0.855f, 0.903f, 0.87f)]
    [TrackClipType(typeof(DialogueClip))]
    [TrackBindingType(typeof(TextMeshProUGUI))]
    public class DialogueTrack : TrackAsset {
        
    }
}
