using System.Collections.Generic;
using MainGame;
using MainGame.DeveloperConsole;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour {
    [Header("Input")] [SerializeField] private InputProvider inputProvider = null;

    [Space, Header("Developer Console")] [SerializeField]
    private List<ConsoleCommand> commands = new List<ConsoleCommand>();

    [Header("Menu")] public GameObject menu;
    public TabGroup tabGroup;

    public int currency = 5;
    public int energy = 10;

    [Space, SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI energyText;

    private void Awake() {
        menu.SetActive(false);
    }

    private void OnEnable() {
        inputProvider.OpenDevConsole += OpenDevConsole;

        inputProvider.OpenMenuWindow += OpenMenuWindow;
        inputProvider.TabRightButtonEvent += OnTabRightButton;
        inputProvider.TabLeftButtonEvent += OnTabLeftButton;
        inputProvider.CloseMenuWindow += CloseMenuWindow;
    }

    private void OnDisable() {
        inputProvider.OpenDevConsole -= OpenDevConsole;

        inputProvider.OpenMenuWindow -= OpenMenuWindow;
        inputProvider.TabRightButtonEvent -= OnTabRightButton;
        inputProvider.TabLeftButtonEvent -= OnTabLeftButton;
        inputProvider.CloseMenuWindow -= CloseMenuWindow;
    }

    // --- Event Listeners --- // 
    private void OpenDevConsole() => DeveloperConsoleBehaviour.GetDevConsole(commands, inputProvider, "/");

    private void OpenMenuWindow() {
        menu.SetActive(true);
        inputProvider.EnableMenuInput();
    }

    private void OnTabRightButton() {
        tabGroup.OnTabRightButton();
    }

    private void OnTabLeftButton() {
        tabGroup.OnTabLeftButton();
    }

    private void CloseMenuWindow() {
        menu.SetActive(false);
        inputProvider.EnableGameplayInput();
    }
}