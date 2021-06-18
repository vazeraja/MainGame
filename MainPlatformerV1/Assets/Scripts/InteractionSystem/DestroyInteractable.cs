using UnityEngine;

namespace MainGame {
    public class DestroyInteractable : InteractableBase {
        public override void OnInteract() {
            base.OnInteract();
            Destroy(gameObject);
        }
    }
}