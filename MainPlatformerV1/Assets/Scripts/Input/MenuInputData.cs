using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "MenuInputData", menuName = "InputData/MenuInputData", order = 0)]
    public class MenuInputData : ScriptableObject {
        [SerializeField] private InputReader inputReader = null;

        [SerializeField] private bool tabRightPressed;
        [SerializeField] private bool tabRightReleased;
        [SerializeField] private bool tabLeftPressed;
        [SerializeField] private bool tabLeftReleased;
        [SerializeField] private bool closeMenuPressed;
        [SerializeField] private bool closeMenuReleased;

        public bool TabRightPressed { get => tabRightPressed; set => tabRightPressed = value; }
        public bool TabRightReleased { get => tabRightReleased; set => tabRightReleased = value; }
        public bool TabLeftPressed { get => tabLeftPressed; set => tabLeftPressed = value; }
        public bool TabLeftReleased{ get => tabLeftReleased; set => tabLeftReleased = value; }
        public bool CloseMenuPressed{ get => closeMenuPressed; set => closeMenuPressed = value; }
        public bool CloseMenuReleased{ get => closeMenuReleased; set => closeMenuReleased = value; }

        public void RegisterEvents() {
            inputReader.TabRightButtonEventStarted += OnTabRightButtonPressed;
            inputReader.TabRightButtonEventCancelled += OnTabRightButtonReleased;
            inputReader.TabLeftButtonEventStarted += OnTabLeftButtonPressed;
            inputReader.TabLeftButtonEventCancelled += OnCloseMenuInputReleased;
            inputReader.CloseMenuStartedEvent += OnCloseMenuInputPressed;
            inputReader.CloseMenuCancelledEvent += OnCloseMenuInputReleased;
        }

        public void UnregisterEvents() {
            inputReader.TabRightButtonEventStarted -= OnTabRightButtonPressed;
            inputReader.TabRightButtonEventCancelled -= OnTabRightButtonReleased;
            inputReader.TabLeftButtonEventStarted -= OnTabLeftButtonPressed;
            inputReader.TabLeftButtonEventCancelled -= OnTabLeftButtonReleased;
            inputReader.CloseMenuStartedEvent -= OnCloseMenuInputPressed;
            inputReader.CloseMenuCancelledEvent -= OnCloseMenuInputReleased;
        }

        private void OnTabRightButtonPressed() {
            tabRightPressed = true;
            tabRightReleased = false;
        }

        private void OnTabRightButtonReleased() {
            tabRightPressed = false;
            tabRightReleased = true;
        }

        private void OnTabLeftButtonPressed() {
            tabLeftPressed = true;
            tabLeftReleased = false;
        }

        private void OnTabLeftButtonReleased() {
            tabLeftPressed = false;
            tabLeftReleased = true;
        }

        private void OnCloseMenuInputPressed() {
            closeMenuPressed = true;
            closeMenuReleased = false;
        }

        private void OnCloseMenuInputReleased() {
            closeMenuPressed = false;
            closeMenuReleased = true;
        }

        public void Reset() {
            tabRightPressed = false;
            tabRightReleased = false;
            tabLeftPressed = false;
            tabLeftReleased = false;
            closeMenuPressed = false;
            closeMenuReleased = false;
        }
    }
}