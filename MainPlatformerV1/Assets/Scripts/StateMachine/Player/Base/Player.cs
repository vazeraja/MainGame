using System.Collections;
using UnityEngine;
using TMPro;

namespace MainGame {

    public class Player : CustomPhysics, IStateMachine<PlayerStateSO> {
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private PlayerData playerData = default;
        [SerializeField] private Weapon weapon = default;

        public Weapon Weapon => weapon;
        public PlayerData PlayerData => playerData;

        [HideInInspector] public int FacingDirection;

        public PlayerStateSO currentState;
        public PlayerStateSO remainState;
        public TextMeshProUGUI currentStateName;

        [HideInInspector] public bool isAnimationFinished;

        [HideInInspector] public Vector2 MovementInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool AttackInput = false;
        [HideInInspector] public bool DashInput = false;
        [HideInInspector] public Vector2 DashKeyboardInput;

        protected override void Awake() {
            base.Awake();
        }

        protected override void OnEnable() {
            base.OnEnable();

            FacingDirection = 1;

            inputReader.moveEvent += OnMove;
            inputReader.jumpEvent += OnJumpInitiated;
            inputReader.jumpCanceledEvent += OnJumpCanceled;
            inputReader.attackEvent += OnAttackInitiated;
            inputReader.attackCanceledEvent += OnAttackCanceled;
            inputReader.dashEvent += OnDashInitiated;
            inputReader.dashCanceledEvent += OnDashCancelled;
            inputReader.dashKeyboardEvent += OnDashKeyboard;
        }
        protected override void OnDisable() {
            base.OnDisable();

            inputReader.moveEvent -= OnMove;
            inputReader.jumpEvent -= OnJumpInitiated;
            inputReader.jumpCanceledEvent -= OnJumpCanceled;
            inputReader.attackEvent -= OnAttackInitiated;
            inputReader.attackCanceledEvent -= OnAttackCanceled;
            inputReader.dashEvent -= OnDashInitiated;
            inputReader.dashCanceledEvent -= OnDashCancelled;
            inputReader.dashKeyboardEvent -= OnDashKeyboard;
        }
        protected override void Start() {
            base.Start();

            currentState.OnStateEnter(this);
        }
        protected override void Update() {
            base.Update();

            currentState.OnLogicUpdate(this);
            currentStateName.text = currentState.stateName;
        }

        /// <summary>
        /// Move to next state only if next state is not equal to remain state
        /// </summary>
        public void TransitionToState(PlayerStateSO nextState) {
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            Anim.SetBool(currentState.animBoolName, false);
            currentState = nextState;
            Anim.SetBool(currentState.animBoolName, true);
            currentState.OnStateEnter(this);
        }
        public void AnimationFinishTrigger() => isAnimationFinished = true;
        public void Flip() {
            FacingDirection *= -1;

            var scale = transform.localScale;
            scale.x *= -1; transform.localScale = scale;
        }


        //---- EVENT LISTENERS ----
        private void OnMove(Vector2 input) => MovementInput = input;
        private void OnJumpInitiated() => JumpInput = true;
        private void OnJumpCanceled() => JumpInput = false;
        private void OnAttackInitiated() => AttackInput = true;
        private void OnAttackCanceled() => AttackInput = false;
        private void OnDashInitiated() => DashInput = true;
        private void OnDashCancelled() => DashInput = false;
        private void OnDashKeyboard(Vector2 input) => DashKeyboardInput = input;
    }
}
