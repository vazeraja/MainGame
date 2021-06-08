using System.Net.NetworkInformation;
using UnityEngine;
namespace MainGame {

	public interface IInteractable {
		float HoldDuration { get; }
		bool HoldInteract { get; }
		bool MultipleUse { get; }
		bool IsInteractable { get; }

		void OnInteract();
	}

	public class InteractableBase : MonoBehaviour, IInteractable {

		[Header("Interactable Settings")] 
		[SerializeField] private float requiredDistance;
		
		[SerializeField] private float holdDuration;
		[SerializeField] private bool holdInteract;
		[SerializeField] private bool multipleUse;
		[SerializeField] private bool isInteractable;

		public float RequiredDistance => requiredDistance;
		public float HoldDuration => holdDuration;
		public bool HoldInteract => holdInteract;
		public bool MultipleUse => multipleUse;
		public bool IsInteractable => isInteractable;

		public virtual void OnInteract() {
			Debug.Log("INTERACTED: " + gameObject.name);
		}
		
		
	}
}
