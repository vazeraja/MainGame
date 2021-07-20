using System.Collections.Generic;
using MainGame;
using MainGame.DeveloperConsole;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour {
    [Header("Input")] [SerializeField] private InputReader inputReader = null;

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
        inputReader.OpenDevConsole += OpenDevConsole;

        inputReader.OpenMenuWindow += OpenMenuWindow;
        inputReader.TabRightButtonEvent += OnTabRightButton;
        inputReader.TabLeftButtonEvent += OnTabLeftButton;
        inputReader.CloseMenuWindow += CloseMenuWindow;
    }

    private void OnDisable() {
        inputReader.OpenDevConsole -= OpenDevConsole;

        inputReader.OpenMenuWindow -= OpenMenuWindow;
        inputReader.TabRightButtonEvent -= OnTabRightButton;
        inputReader.TabLeftButtonEvent -= OnTabLeftButton;
        inputReader.CloseMenuWindow -= CloseMenuWindow;
    }

    // --- Event Listeners --- // 
    private void OpenDevConsole() => DeveloperConsoleBehaviour.GetDevConsole(commands, inputReader, "/");

    private void OpenMenuWindow() {
        menu.SetActive(true);
        inputReader.EnableMenuInput();
    }

    private void OnTabRightButton() {
        tabGroup.OnTabRightButton();
    }

    private void OnTabLeftButton() {
        tabGroup.OnTabLeftButton();
    }

    private void CloseMenuWindow() {
        menu.SetActive(false);
        inputReader.EnableGameplayInput();
    }
}