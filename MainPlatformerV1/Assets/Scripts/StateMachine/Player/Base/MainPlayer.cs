using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

namespace MainGame {

	public class MainPlayer : CustomPhysics {

		[Header("Scriptable Objects")]
		[SerializeField] private InputReader inputReader = default;
		[SerializeField] private InteractionInputData interactionInputData = default;
		[SerializeField] private PlayerInputData playerInputData = default;

		[SerializeField] private PlayerEvent playerInitialized;

		[Header("State Machine")]
		public PlayerStateSO currentState;
		public PlayerStateSO remainState;
		private PlayerStateSO lastState;
		public Action<PlayerStateSO> OnStateTransition;
		public Optional<TextMeshProUGUI> stateName;

		// public CharacterStat Strength;

		private readonly Dictionary<string, float> AnimationStates = new Dictionary<string, float>();

		protected override void Awake(){
			base.Awake();
			inputReader.EnableGameplayInput();
		}

		#region Built-In Methods
		protected override void OnEnable(){
			base.OnEnable();
			
			playerInputData.RegisterEvents();
			interactionInputData.RegisterEvents();

			OnStateTransition += TransitionToState;
			OnStateTransition += _ => ChangeStateName();
		}
		protected override void OnDisable(){
			base.OnDisable();

			playerInputData.UnregisterEvents();
			interactionInputData.UnregisterEvents();

			OnStateTransition -= TransitionToState;
			OnStateTransition -= _ => ChangeStateName();
		}
		protected override void Start(){
			base.Start();

			playerInitialized.Raise(this);
			playerInputData.Reset();
			interactionInputData.Reset();

			currentState.OnStateEnter(this);

			GetAnimationClipTimes();
		}
		protected override void Update(){
			base.Update();

			currentState.OnLogicUpdate(this);
		}
		#endregion

		#region Animation
		public void AnimationFinishTrigger() => playerInputData.IsAnimationFinished = true; // Used as an Animation Event

		private void GetAnimationClipTimes(){
			foreach (var animationClip in Anim.runtimeAnimatorController.animationClips) {
				if (!AnimationStates.ContainsKey(animationClip.name))
					AnimationStates.Add(animationClip.name, animationClip.length);
			}
		}
		#endregion

		#region Event Listeners
		public void TransitionToState(PlayerStateSO nextState){
			if (nextState == remainState)
				return;

			currentState.OnStateExit(this);
			lastState = currentState;
			if (currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, false);
			currentState = nextState;
			if (currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, true);
			currentState.OnStateEnter(this);
		}
		private void ChangeStateName(){
			if (stateName.Enabled) stateName.Value.text = currentState.stateName;
		}
		#endregion

		#region Other
		public void Flip(){
			playerInputData.FacingDirection *= -1;
			var t = transform;
			var scale = t.localScale;
			scale.x *= -1; t.localScale = scale;
		}
		#endregion

		protected override void OnDrawGizmos(){
			base.OnDrawGizmos();
			Gizmos.DrawWireSphere(Collider2D.bounds.center, 0.2f);
		}

	}
}
