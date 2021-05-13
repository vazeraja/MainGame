using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame {

    [CreateAssetMenu(fileName = "DialoguePresets", menuName = "Dialogue/DialoguePresets", order = 0)]
    public class DialoguePresets : ScriptableObject {

        public enum PresetTypes {
            SMALL, MEDIUM, BIG
        }

        public float letterSpacing, wordSpacing, lineSpacing, letterSize, indentLeft, indentRight, indentTop, indentBottom;

        public void SetPreset(PresetTypes preset) {
            switch (preset) {
                case PresetTypes.SMALL: SetSmallPreset(); break;
                case PresetTypes.MEDIUM: break;
                case PresetTypes.BIG: SetBigPreset(); break;
                default: break;
            }
        }

        private void SetBigPreset() {
            letterSpacing = 7f;
            wordSpacing = 10f;
            lineSpacing = 110f;
            letterSize = 80f;
            indentLeft = 50f;
            indentRight = 50f;
            indentTop = -120f;
            indentBottom = 0f;
        }

        public void SetSmallPreset() {
            letterSpacing = 4f;
            wordSpacing = 10f;
            lineSpacing = 70f;
            letterSize = 50f;
            indentLeft = 40f;
            indentRight = 40f;
            indentTop = -60f;
            indentBottom = 0f;
        }
    }

}
