using System.Collections.Generic;
using MainGame.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MainGame.DeveloperConsole {
    public class DeveloperConsoleBehaviour : RuntimeSingleton<DeveloperConsoleBehaviour> {
        
        public static void GetDevConsole(IEnumerable<ConsoleCommand> com, InputReader input, string slash){
            Instance.commands.AddRange(com);
            Instance.inputReader = input;
            Instance.prefix = slash;
            Instance.CreateDevConsole();
        }

        private string prefix = string.Empty;
        private InputReader inputReader = null;
        private readonly List<ConsoleCommand> commands = new List<ConsoleCommand>();

        private DeveloperConsole developerConsole;
        private DeveloperConsole DeveloperConsole => developerConsole ??= new DeveloperConsole(prefix, commands);

        #region Developer Console Variables
        private GameObject canvas;
        private TMP_InputField inputField;
        private Image inputFieldBackground;
        private TextMeshProUGUI placeholderText;
        private TextMeshProUGUI textComponent;
        private RectTransform textAreaRectTransform;
        #endregion

        public void ProcessCommand(string inputValue){
            DeveloperConsole.ProcessCommand(inputValue);
            inputField.text = string.Empty;
            inputReader.EnableGameplayInput();
            Destroy(canvas);
        }

        private void CreateDevConsole(){
            if (canvas != null) return;
            
            inputReader.DisableAllInput();

            canvas = new GameObject("DeveloperConsoleCanvas");
            var developerConsoleCanvas = canvas.AddComponent<Canvas>();
            var canvasScaler = canvas.AddComponent<CanvasScaler>();
            var graphicRaycaster = canvas.AddComponent<GraphicRaycaster>();
            var rt1 = canvas.GetComponent<RectTransform>();
            canvas.layer = LayerMask.NameToLayer("UI");

            developerConsoleCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            developerConsoleCanvas.pixelPerfect = true;
            var additionalShaderChannels = developerConsoleCanvas.additionalShaderChannels;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            developerConsoleCanvas.additionalShaderChannels = additionalShaderChannels;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(3840, 2160);

            canvas.transform.SetParent(gameObject.transform);

            var consoleBackground = new GameObject("ConsoleBackground");
            var backgroundImage = consoleBackground.AddComponent<Image>();
            var rt2 = consoleBackground.GetComponent<RectTransform>();
            backgroundImage.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            backgroundImage.color = new Color(0f, 0f, 0f, 0.53f);
            backgroundImage.type = Image.Type.Sliced;
            rt2.SetAndStretchToParentSize(rt1);

            consoleBackground.transform.SetParent(canvas.transform);

            var inputFieldGO = new GameObject("InputField_ConsoleCommands");
            inputFieldBackground = inputFieldGO.AddComponent<Image>();
            var rt3 = inputFieldGO.GetComponent<RectTransform>();
            inputFieldBackground.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
            inputFieldBackground.color = new Color(0f, 0f, 0f, 0.53f);
            inputFieldBackground.type = Image.Type.Sliced;

            rt3.pivot = new Vector2(0.5f, 0);
            rt3.anchorMin = new Vector2(0, 0);
            rt3.anchorMax = new Vector2(1f, 0);
            rt3.offsetMin = new Vector2(0, 0);
            rt3.offsetMax = new Vector2(3840, 152.6348f);

            inputField = inputFieldGO.AddComponent<TMP_InputField>();

            inputFieldGO.transform.SetParent(consoleBackground.transform);

            var textArea = new GameObject("Text Area");
            var rectMask = textArea.AddComponent<RectMask2D>();
            textAreaRectTransform = textArea.GetComponent<RectTransform>();
            rectMask.padding = new Vector4(-8f, -5f, -8f, -5f);
            textAreaRectTransform.pivot = new Vector2(0.5f, 0.5f);
            textAreaRectTransform.anchorMin = new Vector2(0, 0);
            textAreaRectTransform.anchorMax = new Vector2(1f, 1f);
            textAreaRectTransform.offsetMin = new Vector2(10f, 6f);
            textAreaRectTransform.offsetMax = new Vector2(3830f, 145.634f);

            textArea.transform.SetParent(inputFieldGO.transform);

            var placeholder = new GameObject("Placeholder");
            placeholderText = placeholder.AddComponent<TextMeshProUGUI>();
            var rt5 = placeholder.GetComponent<RectTransform>();
            rt5.pivot = new Vector2(.5f, 0.5f);
            rt5.anchorMin = new Vector2(0, 0);
            rt5.anchorMax = new Vector2(1f, 1f);
            rt5.offsetMin = new Vector2(10, 6.1f);
            rt5.offsetMax = new Vector2(3830f, 145.6340179f);
            placeholder.transform.SetParent(textArea.transform);

            var text = new GameObject("Text");
            textComponent = text.AddComponent<TextMeshProUGUI>();
            var rt6 = text.GetComponent<RectTransform>();
            rt6.pivot = new Vector2(.5f, 0.5f);
            rt6.anchorMin = new Vector2(0, 0);
            rt6.anchorMax = new Vector2(1f, 1f);
            rt6.offsetMin = new Vector2(10, 6.1f);
            rt6.offsetMax = new Vector2(3830f, 145.6340179f);
            text.transform.SetParent(textArea.transform);

            SetInputField();
        }
        private void SetInputField(){
            inputField.targetGraphic = inputFieldBackground;
            inputField.textViewport = textAreaRectTransform;
            inputField.textComponent = textComponent;
            inputField.placeholder = placeholderText;
            inputField.pointSize = 130f;
            
            inputField.onEndEdit.AddListener((x) => ProcessCommand(inputField.text));
            inputField.ActivateInputField();
        }

    }
}
