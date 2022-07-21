using TN.Extensions;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "InteractionLogic", menuName = "InteractionSystem/InteractionLogic", order = 0)]
    public class InteractionLogic : ScriptableObject {
        //[SerializeField] private PlayerData playerData;
        [SerializeField] private InteractionInputData interactionInputData;
        [SerializeField] private InteractionData interactionData;
        [SerializeField] private LayerMask interactableLayer;

        private InteractableBase interactable;

        public void UpdateInteractable(PlayerController player, Vector3 center) {
            CheckForInteractable(player, center);
            CheckForInteractableInput(player);
        }

        private void CheckForInteractable(PlayerController player, Vector3 center) {
            // Shoot a raycast to the right
            bool hitSomething = Helper.Raycast(center, player.transform.right, 5f, interactableLayer, out var ray);
            if (hitSomething) {
                interactable = ray.transform.GetComponent<InteractableBase>();
                if (interactable != null) {
                    if (interactionData.IsEmpty()) {
                        interactionData.Interactable = interactable;
                    }
                    else {
                        if (!interactionData.IsSameInteractable(interactable)) {
                            interactionData.Interactable = interactable;
                        }
                    }
                }
            }
            else {
                interactionData.ResetData();
                interactable = null;
            }

            Debug.DrawRay(center, player.transform.right * 5f,
                hitSomething ? Color.green : Color.red);
            
        }

        private void CheckForInteractableInput(PlayerController player) {
            if (interactionData.IsEmpty() || !interactionInputData.isInteracting ||
                !interactionData.Interactable.IsInteractable) return;

            var distanceBetweenInteractable = interactable.transform.position.x - player.transform.position.x;
            Debug.Log(distanceBetweenInteractable);
            if (distanceBetweenInteractable >= interactable.RequiredDistance) return;

            if (interactionData.Interactable.HoldInteract) {
                interactionInputData.holdTimer += Time.deltaTime;
                if (!(interactionInputData.holdTimer >= interactionData.Interactable.HoldDuration)) return;
                interactionData.Interact();
                interactionInputData.isInteracting = false;
            }
            else {
                interactionData.Interact();
                interactionInputData.isInteracting = false;
            }
        }
    }
}