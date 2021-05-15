using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "DialoguePresetValues", menuName = "Dialogue/DialoguePresetValues", order = 0)]
    public class DialoguePresetValues : ScriptableObject {
        public string presetName;
        public float letterSpacing = 4f;
        public float wordSpacing = 10f;
        public float lineSpacing = 70f;
        public float letterSize = 50f;
        public float indentLeft = 40f;
        public float indentRight = 40f;
        public float indentTop = -60f;
        public float indentBottom = 0f;
    }
}