using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame {
    [CreateAssetMenu(fileName = "DialoguePresets", menuName = "Dialogue/DialoguePresets", order = 0)]
    public class DialoguePresets : ScriptableObject {
        public enum PresetTypes {
            Small,
            Medium,
            Big,
            None,
        }

        public DialoguePresetValues[] presets;

        public float letterSpacing,
            wordSpacing,
            lineSpacing,
            letterSize,
            indentLeft,
            indentRight,
            indentTop,
            indentBottom;

        public void SetPreset(PresetTypes preset){
            switch (preset) {
                case PresetTypes.Small:
                    SetPreset(presets[0]);
                    break;
                case PresetTypes.Medium:
                    SetPreset(presets[1]);
                    break;
                case PresetTypes.Big:
                    SetPreset(presets[2]);
                    break;
                case PresetTypes.None: break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preset), preset, null);
            }
        }

        private void SetPreset(DialoguePresetValues presetValues) {
            letterSpacing = presetValues.letterSpacing;
            wordSpacing = presetValues.wordSpacing;
            lineSpacing = presetValues.lineSpacing;
            letterSize = presetValues.letterSize;
            indentLeft = presetValues.indentLeft;
            indentRight = presetValues.indentRight;
            indentTop = presetValues.indentTop;
            indentBottom = presetValues.indentBottom;
        }
    }
}