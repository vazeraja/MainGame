using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace MainGame.Timeline.CustomDialogueTrack {

    public class DialogueBehaviour : PlayableBehaviour {

        public enum DialogueType { DIALOGUE, RESPONSE }

        [HideInInspector] public bool hasToPause = false;
        [HideInInspector] public DialogueObject dialogue;
        [HideInInspector] public int index = 0;
        [HideInInspector] public DialogueType dialogueType = default;
        [HideInInspector] public InputReader inputReader = default;
        private SpriteLetterSystem SPL;


        private bool clipPlayed = false;
        private bool pauseScheduled = false;
        private bool hasPlayed = false;
        private PlayableDirector director;

        public override void OnPlayableCreate(Playable playable) {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {

            if (!hasPlayed) {

                director.playableGraph.GetRootPlayable(0).SetSpeed(1d);

                inputReader.AdvanceDialogueEvent += AdvanceDialogue;
                inputReader.AdvanceDialogueEvent += SkipDialogue;

                SPL = playerData as SpriteLetterSystem;
                
                if (SPL is {}) {
                    SPL.dialogueBoxRT.gameObject.SetActive(true);

                    inputReader.EnableDialogueInput();

                    try {
                        switch (dialogueType) {
                            case DialogueType.DIALOGUE:
                                SPL.GenerateSpriteText(dialogue.dialogue[index]);
                                break;
                            case DialogueType.RESPONSE:
                                SPL.GenerateSpriteText(dialogue.responseOptions[index].text);
                                break;
                        }
                    }
                    catch {
                        Debug.Log("index out of bounds for dialogue array.");
                    }
                }

                hasPlayed = true;
            }

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
                hasPlayed = false;
            }

            clipPlayed = false;
        }

        public override void OnGraphStop(Playable playable) {
            inputReader.EnableGameplayInput();
            inputReader.AdvanceDialogueEvent -= AdvanceDialogue;
            inputReader.AdvanceDialogueEvent -= SkipDialogue;
            if (SPL is {})
                SPL.dialogueBoxRT.gameObject.SetActive(false);
        }

        private void AdvanceDialogue() {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
        private void SkipDialogue() {
            director.playableGraph.GetRootPlayable(0).SetSpeed(5d);
        }
        // private void SkipCutscene() {
        //     director.playableGraph.GetRootPlayable(0).SetTime(director.playableGraph.GetRootPlayable(0).GetDuration());
        // }

    }
}
