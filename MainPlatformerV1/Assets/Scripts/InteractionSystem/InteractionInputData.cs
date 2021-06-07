using System;
using UnityEngine;
namespace MainGame {

    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData", order = 0)]
    public class InteractionInputData : ScriptableObject {

        [SerializeField] private InputReader inputReader = null;
        
        [Space]
        [SerializeField] private bool interactionClicked;
        [SerializeField] private bool interactionReleased;

        // ReSharper disable once ConvertToAutoProperty
        public bool InteractedClicked { get => interactionClicked; set => interactionClicked = value; }
        // ReSharper disable once ConvertToAutoProperty
        public bool InteractedRelease { get => interactionReleased; set => interactionReleased = value; }

        public void RegisterEvents(){
            inputReader.InteractionStartedEvent += OnInteractionClicked;
            inputReader.InteractionCancelledEvent += OnInteractionReleased;
        }
        public void UnregisterEvents(){
            inputReader.InteractionStartedEvent -= OnInteractionClicked;
            inputReader.InteractionCancelledEvent -= OnInteractionReleased;
        }

        private void OnInteractionClicked(){
            InteractedClicked = true;
            InteractedRelease = false;
        }
        private void OnInteractionReleased(){
            InteractedClicked = false;
            InteractedRelease = true;
        }

        public void Reset(){
            interactionClicked = false;
            interactionReleased = false;
        }
        
        
    }
}
