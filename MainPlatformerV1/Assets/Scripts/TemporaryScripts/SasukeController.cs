using System;
using Aarthificial.Reanimation;
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
        public PlayerInputData playerInputData;
        
        [Header("State Machine")] 
        public SasukeStateSO currentState;
        public SasukeStateSO remainState;

        [Header("Walking")] 
        [SerializeField] private float maxWalkCos = 0.5f;
        [SerializeField] public float walkSpeed = 7;

        [Header("Jumping")] 
        [SerializeField]
        public float firstJumpSpeed = 8;
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

        public SasukeState State { get; set; } = SasukeState.Movement;
        public Vector2 DesiredDirection { get; private set; }
        public int FacingDirection { get; set; } = 1;

        public bool IsGrounded => groundContact.HasValue;
        public Vector2 Velocity => rigidbody2D.velocity;
        public float AttackCompletion => dashStopwatch.Completion;
        public float JumpCompletion => jumpStopwatch.Completion;
        public bool IsJumping => !jumpStopwatch.IsFinished;
        public bool IsFirstJump => jumpsLeft == numberOfJumps - 1;

        [HideInInspector] public new Rigidbody2D rigidbody2D;
        private ContactFilter2D contactFilter;
        public ContactPoint2D? groundContact;
        public ContactPoint2D? ceilingContact;
        public ContactPoint2D? wallContact;
        private readonly ContactPoint2D[] contacts = new ContactPoint2D[16];

        [HideInInspector] public bool wantsToJump;
        [HideInInspector] public bool wasOnTheGround;
        [HideInInspector] public bool canDash;
        [HideInInspector] public int jumpsLeft;
        [HideInInspector] public int enemyLayer;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            
            contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
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
            currentState.OnStateUpdate(this);
        }

        private void FixedUpdate()
        {
            FindContacts();
            currentState.OnStatePhysicsUpdate(this);
        }

        #region Events

        private void OnMove(Vector2 value) => DesiredDirection = value;

        private void OnJump(float value)
        {
            wantsToJump = value > 0.5f;

            if (wantsToJump)
                RequestJump();
            else
                jumpStopwatch.Reset();
        }

        private void OnDash()
        {
            EnterDashState();
        }
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


        private void RequestJump()
        {
            if (State != SasukeState.Movement || jumpsLeft <= 0)
                return;

            jumpsLeft--;
            jumpStopwatch.Split();
        }

        private void FindContacts()
        {
            groundContact = null;
            ceilingContact = null;
            wallContact = null;

            float groundProjection = maxWalkCos;
            float wallProjection = maxWalkCos;
            float ceilingProjection = -maxWalkCos;

            int numberOfContacts = rigidbody2D.GetContacts(contactFilter, contacts);
            for (var i = 0; i < numberOfContacts; i++)
            {
                var contact = contacts[i];
                float projection = Vector2.Dot(Vector2.up, contact.normal);

                if (projection > groundProjection)
                {
                    groundContact = contact;
                    groundProjection = projection;
                }
                else if (projection < ceilingProjection)
                {
                    ceilingContact = contact;
                    ceilingProjection = projection;
                }
                else if (projection <= wallProjection)
                {
                    wallContact = contact;
                    wallProjection = projection;
                }
            }
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