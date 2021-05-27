using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;


namespace MainGame {

    public class Player : CustomPhysics, IStateMachine<PlayerStateSO> {

        #region Variables
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private PlayerData playerData = default;

        public PlayerData PlayerData { get => playerData; set => playerData = value; }
        public InputReader InputReader { get => inputReader; set => inputReader = value; }

        // public CharacterStat Strength;

        public PlayerStateSO currentState;
        public PlayerStateSO remainState;
        private PlayerStateSO lastState;
        public TextMeshProUGUI currentStateName;

        [HideInInspector] public int FacingDirection;
        [HideInInspector] public bool isAnimationFinished;

        [HideInInspector] public Vector2 MovementInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool AttackInput = false;
        [HideInInspector] public bool DashInput = false;
        [HideInInspector] public Vector2 DashKeyboardInput;

        private readonly Dictionary<string, float> AnimationStates = new Dictionary<string, float>();

        private UniversalAdditionalCameraData camData;
        #endregion

        #region Unity Callback Functions
        protected override void OnEnable(){
            base.OnEnable();

            GameManager.Instance.RegisterPlayer(this);

            FacingDirection = 1;

            InputReader.MoveEvent += OnMove;
            InputReader.JumpEvent += OnJumpInitiated;
            InputReader.JumpCanceledEvent += OnJumpCanceled;
            InputReader.DashEvent += OnDashInitiated;
            InputReader.DashCanceledEvent += OnDashCancelled;
            InputReader.DashKeyboardEvent += OnDashKeyboard;
            InputReader.AttackEvent += OnAttackInitiated;
            InputReader.AttackCanceledEvent += OnAttackCanceled;
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
        }
        protected override void Start(){
            base.Start();

            currentState.OnStateEnter(this);
            UpdateAnimClipTimes();
        }
        protected override void Update(){
            base.Update();

            currentState.OnLogicUpdate(this);
            // currentStateName.text = currentState.stateName;
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
            Anim.SetBool(currentState.animBoolName, false);
            currentState = nextState;
            Anim.SetBool(currentState.animBoolName, true);
            currentState.OnStateEnter(this);
        }
        #endregion

        #region Animation
        public void AnimationFinishTrigger() => isAnimationFinished = true;

        private void UpdateAnimClipTimes(){
            var clips = Anim.runtimeAnimatorController.animationClips;
            foreach (var animationClip in clips) // Add run animation clip to dictionary with length of clip in seconds
                switch (animationClip.name) {
                    case "player_idle":
                        AnimationStates.Add($"player_idle", animationClip.length); // Multiply by framerate to get amount of frames in clip
                        break;
                    case "player_run":
                        AnimationStates.Add($"player_run", animationClip.length);
                        break;
                    case "player_saber-green":
                        AnimationStates.Add($"player_saber-green", animationClip.length);
                        break;
                    case "player_saber-orange":
                        AnimationStates.Add($"player_saber-orange", animationClip.length);
                        break;
                    case "player_saber-purple":
                        AnimationStates.Add($"player_saber-purple", animationClip.length);
                        break;
                    case "player_saber-fire":
                        AnimationStates.Add($"player_saber-fire", animationClip.length);
                        break;
                }
            //AnimationStates.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Debug.Log);
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
