using UnityEngine;
using Utils;

namespace MainGame {

    public enum SasukeState {
        Movement = 0,
        Dash = 1,
        Hit = 2,
    }
    public class SasukeController : MonoBehaviour {
        
        [Header("Input")] 
        public InputReader inputReader;

        [Header("State Machine")] 
        public SasukeStateSO currentState;
        public SasukeStateSO remainState;

        [Header("Walking")]
        [SerializeField] public float walkSpeed = 7;

        [Header("Jumping")] 
        [SerializeField] public float firstJumpSpeed = 8;
        [SerializeField] public float jumpSpeed = 3;
        [SerializeField] public float fallSpeed = 12;
        [SerializeField] public int numberOfJumps = 2;
        [SerializeField] public AnimationCurve jumpFallOff = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] public FixedStopwatch jumpStopwatch = new FixedStopwatch();

        [Header("Getting a whooping")] 
        public Vector2 hitForce;
        public FixedStopwatch hitStopwatch = new FixedStopwatch();
        public Collision2D CollisionData;

        [Header("Giving a whooping")] 
        public float dashSpeed = 12;
        public FixedStopwatch dashStopwatch = new FixedStopwatch();

        public CollisionInfo CollisionInfo { get; private set; }
        public SasukeState State { get; set; } = SasukeState.Movement;
        public Vector2 DesiredDirection { get; private set; }
        public int FacingDirection { get; set; } = 1;

        public float AttackCompletion => dashStopwatch.Completion;
        public float JumpCompletion => jumpStopwatch.Completion;
        public bool IsJumping => !jumpStopwatch.IsFinished;
        public bool IsFirstJump => jumpsLeft == numberOfJumps - 1;
        

        [HideInInspector] public bool wantsToJump;
        [HideInInspector] public bool wasOnTheGround;
        [HideInInspector] public bool canDash;
        [HideInInspector] public int jumpsLeft;
        [HideInInspector] public int enemyLayer;

        private void Awake() {
            CollisionInfo = GetComponent<CollisionInfo>();
            enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void OnEnable() {
            inputReader.MoveEvent += OnMove;
            inputReader.FJumpEvent += OnJump;
            inputReader.AttackEvent += OnDash;
            
            SasukeStateSO.OnStateTransition += TransitionToState;
        }

        private void OnDisable() {
            inputReader.MoveEvent -= OnMove;
            inputReader.FJumpEvent -= OnJump;
            inputReader.AttackEvent -= OnDash;
            
            SasukeStateSO.OnStateTransition -= TransitionToState;
        }

        private void Start() {
            currentState.OnStateEnter(this);
        }

        private void Update() {
            currentState.OnStateLogicUpdate(this);
        }

        private void FixedUpdate() {
            currentState.OnStatePhysicsUpdate(this);
        }

        #region Events

        private void OnMove(Vector2 value) => DesiredDirection = value;

        private void OnJump(float value) {
            wantsToJump = value > 0.5f;

            if (wantsToJump) {
                if (State != SasukeState.Movement || jumpsLeft <= 0)
                    return;

                jumpsLeft--;
                jumpStopwatch.Split();
            }
            else {
                jumpStopwatch.Reset();
            }
        }

        private void OnDash() => EnterDashState();

        private void TransitionToState(SasukeStateSO nextState) {
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            currentState = nextState;
            currentState.OnStateEnter(this);
        }
        #endregion

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != enemyLayer) return;
            CollisionData = other;
            EnterHitState();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.layer != enemyLayer || State == SasukeState.Hit) return;
            CollisionData = other;
            EnterHitState();
        }

        #region States

        private void EnterHitState()
        {
            if (State != SasukeState.Hit && !hitStopwatch.IsReady) return;
            State = SasukeState.Hit;
        }

        private void EnterDashState()
        {
            if (State != SasukeState.Movement || !dashStopwatch.IsReady || !canDash) 
                return;
            State = SasukeState.Dash;
        }

        public void EnterMovementState() {
            State = SasukeState.Movement;
        }
        
        #endregion
    }
}