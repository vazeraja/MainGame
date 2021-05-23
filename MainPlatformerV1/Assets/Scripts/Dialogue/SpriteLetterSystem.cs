using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MainGame.Utils;


namespace MainGame {

    //Attatch this to something, ideally something in a unity canvas
    public class SpriteLetterSystem : MonoBehaviour {
        private enum TextEffect { None, Wavy, Shaky }

        #region Variables
        // public
        [SerializeField] private Texture2D charSheet; //This should be the resource you want to load, Make sure that the image is in Assets/Resources ideally. Its a folder that unity can load things from using Resources.LoadAll etc
        [SerializeField] private GameObject letterObject;
        [SerializeField] private DialogueObject dialogue;
        [SerializeField] private DialoguePresets dialoguePreset = default;
        [SerializeField] private Color defaultColor = default;


        [Header("Text Effects")]
        [SerializeField] private float wavyStrength = 0.5f;
        [SerializeField] private float shakyStrength = 0.5f;
        // private
        public RectTransform dialogueBoxRT = default;
        private TextEffect activeEffect;
        private Color activeColor;
        private List<GameObject> letterObjects = new List<GameObject>();
        private Dictionary<CharSpriteData, TextEffect> fxChars = new Dictionary<CharSpriteData, TextEffect>();
        private Dictionary<char, CharData> loadedFont;
        #endregion

        /// <summary>
        /// Container for character sprite data
        /// </summary>
        private struct CharSpriteData {
            public Transform transform; // gameobject component
            public Vector2 position => transform.position;
            public Image image; // gameobject component
            public RectTransform rectTransform; // gameobject component
            public float rightOffset;
            public float leftOffset;

            public CharSpriteData(Transform t, Image i, Color c, RectTransform rt, float right, float left){
                transform = t;
                image = i;
                image.color = c;
                rectTransform = rt;
                rightOffset = right;
                leftOffset = left;
            }

        }


#if UNITY_EDITOR
        private void OnGUI(){
            if (GUI.Button(new Rect(0, 0, 200, 50), "Generate Text")) {
                GenerateSpriteText(dialogue.dialogue[0]);
            }
            else if (GUI.Button(new Rect(0, 50, 200, 50), "Clear Text")) {
                gameObject.DestroyAllChildren<RectTransform>();
                fxChars.Clear();
            }
        }
#endif

        private void Awake(){
            loadedFont = FontLoader.LoadFontResource(charSheet);
        }

        private void FixedUpdate(){
            DoTextEffects();
        }


        /// <summary>
        /// Generates characters sprites from string and applies text effects
        /// </summary>
        public void GenerateSpriteText(string textToGenerate){

            dialoguePreset.SetPreset(dialogue.preset);

            gameObject.DestroyAllChildren<RectTransform>();
            fxChars.Clear();

            if (letterObject == null) return;

            // The sprite text generator object should be place at the top left corner of the text box

            activeColor = defaultColor;

            float scale = dialoguePreset.letterSize / 100f;

            var dBoxRect = dialogueBoxRT.rect;

            float xPosition = dialoguePreset.indentLeft;
            float yPosition = dialoguePreset.indentTop;

            bool inTag = false;

            int wordCount = 0;

            var wordList = new List<CharSpriteData>();

            foreach ((char currentCharacter, int index) in textToGenerate.WithIndex()) {

                CheckTag(textToGenerate, currentCharacter, index, ref inTag);

                // Out of dialogue box bounds Y
                if (yPosition < -dBoxRect.height + dialoguePreset.indentBottom)
                    continue;

                if (!inTag) {
                    if (currentCharacter == ' ') {
                        xPosition += dialoguePreset.letterSpacing * dialoguePreset.wordSpacing * scale;
                        wordCount++;
                        wordList.Clear();
                        continue;
                    }
                    var currentCharacterData = loadedFont[currentCharacter];

                    xPosition += currentCharacterData.LeftOffset * dialoguePreset.letterSpacing * scale;

                    // Create new game object
                    var newLetterSprite = CreateNewLetter(currentCharacterData, xPosition, yPosition, index);
                    newLetterSprite.transform.localScale = new Vector3(scale, scale, 1f);
                    // Debug.Log(xPosition + " " + index);

                    letterObjects.Add(newLetterSprite);

                    // set active color here so we can wrap other effects in color tags
                    var charData = new CharSpriteData(
                        newLetterSprite.transform,
                        newLetterSprite.GetComponent<Image>(),
                        activeColor,
                        newLetterSprite.GetComponent<RectTransform>(),
                        currentCharacterData.RightOffset, currentCharacterData.LeftOffset
                    );

                    wordList.Add(charData);

                    fxChars.Add(charData, activeEffect);

                    xPosition += currentCharacterData.RightOffset * dialoguePreset.letterSpacing * scale;

                    if (xPosition >= dBoxRect.width - dialoguePreset.indentRight) {
                        yPosition -= dialoguePreset.lineSpacing; // go to next line
                        xPosition = dialoguePreset.indentLeft; // reset x position

                        // puts characters of unfinished word on the next line
                        if (wordList.Count > 0)
                            foreach (var letterData in wordList) {
                                xPosition += letterData.leftOffset * dialoguePreset.letterSpacing * scale;
                                letterData.transform.localPosition = new Vector3(xPosition, yPosition, 1f);
                                xPosition += letterData.rightOffset * dialoguePreset.letterSpacing * scale;

                                if (yPosition < -dBoxRect.height) letterData.transform.gameObject.SetActive(false);
                            }
                    }
                }
            }
        }


        /// <summary>
        /// Creates new letter sprite and displays on canvas
        /// </summary>
        private GameObject CreateNewLetter(CharData newCharacter, float positionX, float positionY, int letterNumber){

            //Create new game object
            var newLetterSprite = Instantiate(letterObject, transform);
            var newLetterTransform = newLetterSprite.GetComponent<RectTransform>();
            newLetterSprite.name = "lettersprite_" + letterNumber;
            newLetterTransform.localPosition += new Vector3(positionX, positionY, 0f);
            var newLetterImage = newLetterSprite.GetComponent<Image>();
            newLetterImage.sprite = newCharacter.Sprite;

            return newLetterSprite;
        }


        /// <summary>
        /// Applies tag effects and checks if current char is in tagged element
        /// </summary>
        private void CheckTag(string fullText, char c, int j, ref bool inTag){
            if (c == '<') {
                inTag = true;

                char next = fullText[j + 1];

                if (next != '/') {
                    switch (next) {
                        case 'w':
                            activeEffect = TextEffect.Wavy;
                            break;
                        case 's':
                            activeEffect = TextEffect.Shaky;
                            break;
                        case 'c':
                            SetColorFromText(fullText, j + 4);
                            break;
                    }
                }
                else {
                    activeEffect = TextEffect.None;
                    activeColor = defaultColor;
                }
            }
            else if (j > 0 && fullText[j - 1] == '>') {
                inTag = false;
            }
        }


        /// <summary>
        /// Sets active color to value specified in element tag
        /// </summary>
        private void SetColorFromText(string fullText, int start){
            // c=( 256, 256, 256)

            int end = start;
            int length = 0;

            while (end < fullText.Length && fullText[end] != ')') {
                end++;
                length++;
            }

            // 256, 256, 256
            string str = fullText.Substring(start, length);

            // [256, 256, 256]
            string[] strSplit = str.Split(',');

            // Debug.Log(strSplit.Length + " " + fullText + " " + str);

            if (strSplit.Length != 3 && end != fullText.Length) Debug.LogError("color must follow <c=(0-256,0-256,0-256)></c> format");

            int[] colorParams = strSplit.Select(int.Parse).ToArray();

            activeColor.r = colorParams[0] / 255f;
            activeColor.g = colorParams[1] / 255f;
            activeColor.b = colorParams[2] / 255f;
            activeColor.a = 1;
        }


        /// <summary>
        /// Applies text effects to each text object in the ui canvas
        /// </summary>
        private void DoTextEffects(){
            foreach (var charData in fxChars.Keys) {
                var effect = fxChars[charData];
                var rectTransform = charData.rectTransform;
                var position = charData.position;

                switch (effect) {
                    case TextEffect.Wavy:
                        rectTransform.anchoredPosition += Vector2.up * Mathf.Sin(position.x * 0.1f + 10 * Time.time) * wavyStrength;
                        break;
                    case TextEffect.Shaky:
                        rectTransform.anchoredPosition = position + Random.insideUnitCircle * shakyStrength;
                        break;
                }
            }
        }

    }
}
