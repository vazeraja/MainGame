using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MainGame {
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class MainPlayer : CustomPhysics, IStateMachine<PlayerStateSO> {

        [Header("Scriptable Objects")]
        [Space]
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private PlayerData playerData = default;
        [SerializeField] private PlayerEvent onPlayerInitialized;

        [Header("State")]
        [Space]
        public Action<PlayerStateSO> OnStateTransition;
        public PlayerStateSO currentState;
        public PlayerStateSO remainState;
        private PlayerStateSO lastState;
        
        [Header("Other")]
        [Space]
        [SerializeField] private Optional<TextMeshProUGUI> stateName;

        // public CharacterStat Strength;
        public PlayerData PlayerData { get => playerData; set => playerData = value; }
        public InputReader InputReader { get => inputReader; set => inputReader = value; }

        [HideInInspector] public int FacingDirection = 1;
        [HideInInspector] public bool isAnimationFinished;
        [HideInInspector] public Vector2 MovementInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool AttackInput = false;
        [HideInInspector] public bool DashInput = false;
        [HideInInspector] public Vector2 DashKeyboardInput;

        private readonly Dictionary<string, float> AnimationStates = new Dictionary<string, float>();

        #region Unity Callback Functions
        protected override void Awake(){
            inputReader.EnableGameplayInput();

            InputReader.MoveEvent += OnMove;
            InputReader.JumpEvent += OnJumpInitiated;
            InputReader.JumpCanceledEvent += OnJumpCanceled;
            InputReader.DashEvent += OnDashInitiated;
            InputReader.DashCanceledEvent += OnDashCancelled;
            InputReader.DashKeyboardEvent += OnDashKeyboard;
            InputReader.AttackEvent += OnAttackInitiated;
            InputReader.AttackCanceledEvent += OnAttackCanceled;

            OnStateTransition += TransitionToState;
            OnStateTransition += _ => ChangeStateName();
        }
        protected override void OnEnable(){
            base.OnEnable();
        }
        protected override void OnDisable(){
            base.OnDisable();

            InputReader.MoveEvent -= OnMove;
            InputReader.JumpEvent -= OnJumpInitiated;
            InputReader.JumpCanceledEvent -= OnJumpCanceled;
            InputReader.DashEvent -= OnDashInitiated;
            InputReader.DashCanceledEvent -= OnDashCancelled;
            InputReader.DashKeyboardEvent -= OnDashKeyboard;
            InputReader.AttackEvent -= OnAttackInitiated;
            InputReader.AttackCanceledEvent -= OnAttackCanceled;

            OnStateTransition -= TransitionToState;
            OnStateTransition -= _ => ChangeStateName();
        }
        protected override void Start(){
            base.Start();
            
            onPlayerInitialized.Raise(this);
            currentState.OnStateEnter(this);
            UpdateAnimClipTimes();
        }
        protected override void Update(){
            base.Update();
            currentState.OnLogicUpdate(this);
        }
        #endregion

        #region State Machine
        /// <summary>
        /// Move to next state only if next state is not equal to remain state
        /// </summary>
        public void TransitionToState(PlayerStateSO nextState){
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            lastState = currentState;
            if(currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, false);
            currentState = nextState;
            if (currentState.animBoolName.Enabled) Anim.SetBool(currentState.animBoolName.Value, true);
            currentState.OnStateEnter(this);
        }
        private void ChangeStateName(){
            if (stateName.Enabled) stateName.Value.text = currentState.stateName;
        }
        
        #endregion

        #region Animation
        public void AnimationFinishTrigger() => isAnimationFinished = true;

        private void UpdateAnimClipTimes(){
            var clips = Anim.runtimeAnimatorController.animationClips;
            foreach (var animationClip in clips) {
                if (!AnimationStates.ContainsKey(animationClip.name))
                    AnimationStates.Add(animationClip.name, animationClip.length);
            }
            // AnimationStates.Select(i => $"{i.Key}: {i.Value}").ForEach(Debug.Log);
        }
        #endregion

        #region Event Listeners
        private void OnMove(Vector2 input) => MovementInput = input;
        private void OnJumpInitiated() => JumpInput = true;
        private void OnJumpCanceled() => JumpInput = false;
        private void OnDashInitiated() => DashInput = true;
        private void OnDashCancelled() => DashInput = false;
        private void OnDashKeyboard(Vector2 input) => DashKeyboardInput = input;
        private void OnAttackInitiated() => AttackInput = true;
        private void OnAttackCanceled() => AttackInput = false;
        #endregion

        #region Other
        public void Flip(){
            FacingDirection *= -1;
            var t = transform;
            var s = t.localScale;
            s.x *= -1;
            t.localScale = s;
        }
        #endregion

    }
}
