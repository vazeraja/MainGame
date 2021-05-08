using UnityEngine;
using TMPro;

namespace MainGame {
    public class Player : CustomPhysics {
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private PlayerData playerData = default;
        [SerializeField] private Weapon weapon = default;

        public Weapon Weapon => weapon;
        public PlayerData PlayerData => playerData;
        public Rigidbody2D RB => rb2d;
        public Animator Anim => animator;
        public SpriteRenderer SR => spriteRenderer;

        public Vector2 MovementVelocity { get { return targetVelocity; } set { targetVelocity = value; } }
        public float yVelocity { get { return velocity.y; } set { velocity.y = value; } }
        public bool IsGrounded => grounded;
        [HideInInspector] public int FacingDirection;

        public State_SO currentState;
        public State_SO remainState;
        public TextMeshProUGUI currentStateName;

        [HideInInspector] public bool isAnimationFinished;
        [HideInInspector] public float startTime;


        [HideInInspector] public Vector2 MovementInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool AttackInput = false;
        [HideInInspector] public bool DashInput = false;
        [HideInInspector] public Vector2 DashKeyboardInput;

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
        protected override void Update() {
            base.Update();

            currentState.OnLogicUpdate(this);
            currentStateName.text = currentState.stateName;
        }
        protected override void Start() {
            base.Start();

            currentState.OnStateEnter(this);
            startTime = Time.time;
        }

        /// <summary>
        /// Move to next state only if next state is not equal to remain state
        /// </summary>
        public void TransitionToState(State_SO nextState) {
            if (nextState != remainState) {
                currentState.OnStateExit(this);
                currentState = nextState;
                currentState.OnStateEnter(this);
            }
        }
        public void AnimationFinishTrigger() => isAnimationFinished = true;


        //---- EVENT LISTENERS ----
        private void OnMove(Vector2 input) => MovementInput = input;
        private void OnJumpInitiated() => JumpInput = true;
        private void OnJumpCanceled() => JumpInput = false;
        private void OnAttackInitiated() => AttackInput = true;
        private void OnAttackCanceled() => AttackInput = false;
        private void OnDashInitiated() => DashInput = true;
        private void OnDashCancelled() => DashInput = false;
        private void OnDashKeyboard(Vector2 input) => DashKeyboardInput = input;

        private void OnDrawGizmos() {

        }
    }
}