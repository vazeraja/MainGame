using System;
using System.Linq;
using CharTween;
using DG.Tweening;
using TMPro;
using TN.Extensions;
using UnityEngine;

namespace ThunderNut.CustomDialogue {
    public class DialogueTweener : MonoBehaviour {
        private TMP_Text m_tmpText;
        private float height = 1f;
        private readonly Color defaultColor = Color.black;

        private void Start() {
            // ParseText("Nice Cock");
            m_tmpText = GetComponent<TMP_Text>();
            m_tmpText.text = "<c=(100,200, 69)><w>NICE COCK!</w></c>";
            ParseText(m_tmpText);
        }

        private void ParseText(TMP_Text tmpText) {
            CharTweener tweener = tmpText.GetCharTweener();
            bool inTag = false;
            Action<int, int, CharTweener, Color> tweenFunction = default;
            Color color = defaultColor;
            String temp = "";
            var start = -1;
            var end = -1;
            bool assigned = false;
            foreach (var c in tmpText.text.WithIndex()) {
                var index = CheckTag(tmpText.text, c.item, c.index, ref inTag, ref tweenFunction, ref color);
                if (!inTag) {
                    temp += c.item;
                    if (!assigned) {
                        start = index;
                        assigned = true;
                    }

                    end = Mathf.Max(end, index);
                }
            }

            tweenFunction(start, end, tweener, color);
            Debug.Log(temp);
        }

        /// <summary>
        /// Applies tag effects and checks if current char is in tagged element
        /// </summary>
        private int CheckTag(string fullText, char c, int j, ref bool inTag,
            ref Action<int, int, CharTweener, Color> tweenFunction, ref Color color) {
            if (c == '<') {
                inTag = true;

                char next = fullText[j + 1];
                Color activeColor = defaultColor;

                if (next != '/') {
                    switch (next) {
                        case 'w':
                            // wavy
                            tweenFunction = BounceTweener;
                            break;
                        case 's':
                            // shaky
                            break;
                        case 'm':
                            tweenFunction = MatrixTweener;
                            break;
                        case 'c':
                            // color 
                            color = SetColorFromText(fullText, j + 4);
                            break;
                    }
                }
                else {
                    // activeEffect = TextEffect.None;
                    // activeColor = defaultColor;
                }

                return -1;
            }
            else if (j > 0 && fullText[j - 1] == '>') {
                inTag = false;
                Debug.Log(c + ": " + j);
                return j;
            }
            else if (!inTag) {
                Debug.Log(c + ": " + j);
                return j;
            }

            return -1;
        }

        /// <summary>
        /// Sets active color to value specified in element tag
        /// </summary>
        private Color SetColorFromText(string fullText, int start) {
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

            if (strSplit.Length != 3 && end != fullText.Length)
                Debug.LogError("color must follow <c=(0-256,0-256,0-256)></c> format");

            int[] colorParams = strSplit.Select(int.Parse).ToArray();

            Color activeColor;

            activeColor.r = colorParams[0] / 255f;
            activeColor.g = colorParams[1] / 255f;
            activeColor.b = colorParams[2] / 255f;
            activeColor.a = 1;

            return activeColor;
        }

        private void BounceTweener(int start, int end, CharTweener tweener, Color color) {
            var sequence = DOTween.Sequence();

            for (var i = start; i <= end; ++i) {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float) (end - start + 1));
                var charSequence = DOTween.Sequence();
                charSequence.Append(tweener.DOLocalMoveY(i, height, 0.5f).SetEase(Ease.InOutCubic))
                    .Join(tweener.DOFade(i, 0, 0.5f).From())
                    .Join(tweener.DOScale(i, 0, 0.5f).From().SetEase(Ease.OutBack, 5))
                    .Append(tweener.DOLocalMoveY(i, 0, 0.5f).SetEase(Ease.OutBounce));
                sequence.Insert(timeOffset, charSequence);
                var rotationTween = tweener
                    .DOLocalRotate(i, UnityEngine.Random.onUnitSphere * 360, 2f, RotateMode.Fast)
                    .SetEase(Ease.InExpo)
                    .SetLoops(-1, LoopType.Yoyo);
                rotationTween.fullPosition = timeOffset;
                var colorTween = tweener.DOColor(i, color, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo);
                colorTween.fullPosition = timeOffset;
            }

            sequence.SetLoops(-1, LoopType.Yoyo);
        }


        private void MatrixTweener(int start, int end, CharTweener tweener, Color color) {
            for (var i = start; i <= end; ++i) {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float) (end - start + 1));
                var rotationTween = tweener
                    .DOLocalRotate(i, UnityEngine.Random.onUnitSphere * 360, 2, RotateMode.FastBeyond360)
                    .SetEase(Ease.InBounce).DOTimeScale(0.3f, -1)
                    .SetLoops(-1, LoopType.Incremental);
                rotationTween.fullPosition = timeOffset;
                var colorTween = tweener.DOColor(i, UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1),
                        UnityEngine.Random.Range(0.1f, 0.5f))
                    .SetLoops(-1, LoopType.Yoyo);
                colorTween.fullPosition = timeOffset;
            }
        }
    }
}