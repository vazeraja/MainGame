using System;
using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData", order = 0)]
    public class InteractionInputData : ScriptableObject {

        [SerializeField] private InputReader inputReader = null;
        
        [Space]
        [SerializeField] private bool interactionClicked;
        [SerializeField] private bool interactionReleased;

        [Space] 
        public bool isInteracting;
        public float holdTimer;

        // ReSharper disable once ConvertToAutoProperty
        public bool InteractClicked { get => interactionClicked; set => interactionClicked = value; }
        // ReSharper disable once ConvertToAutoProperty
        public bool InteractReleased { get => interactionReleased; set => interactionReleased = value; }

        public void RegisterEvents(){
            inputReader.InteractionStartedEvent += OnInteractionClicked;
            inputReader.InteractionCancelledEvent += OnInteractionReleased;
        }
        public void UnregisterEvents(){
            inputReader.InteractionStartedEvent -= OnInteractionClicked;
            inputReader.InteractionCancelledEvent -= OnInteractionReleased;
        }

        private void OnInteractionClicked(){
            InteractClicked = true;
            InteractReleased = false;
            isInteracting = true;
            holdTimer = 0;
        }
        private void OnInteractionReleased(){
            InteractClicked = false;
            InteractReleased = true;
            isInteracting = false;
            holdTimer = 0;
        }

        public void Reset(){
            interactionClicked = false;
            interactionReleased = false;
        }
        
        
    }
}
