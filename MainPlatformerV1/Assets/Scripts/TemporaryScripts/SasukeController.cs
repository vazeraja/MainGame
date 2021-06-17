using System;
using Aarthificial.Reanimation;
using UnityEngine;
using Utils;

namespace MainGame {
    public class SasukeController : MonoBehaviour {
        
        [Header("Input")] 
        public InputReader inputReader;
        public PlayerInputData playerInputData;
        
        [Header("State Machine")] 
        public SasukeStateSO currentState;
        public SasukeStateSO remainState;
        
        [Header("Walking")] 
        public float maxWalkCos = 0.2f;
        public float walkSpeed = 7;
        
        [Header("Jumping")] 
        public float firstJumpSpeed = 8;
        public float jumpSpeed = 3;
        public float fallSpeed = 12;
        public int numberOfJumps = 2;
        public AnimationCurve jumpFallOff = AnimationCurve.Linear(0, 1, 1, 0);
        public FixedStopwatch jumpStopwatch = new FixedStopwatch();

        private int FacingDirection { get; set; }
        public bool IsGrounded => groundContact.HasValue;
        public bool IsTouchingCeiling => ceilingContact.HasValue;
        public bool IsTouchingWall => wallContact.HasValue;
        private bool IsJumping => !jumpStopwatch.IsFinished;
        private bool IsFirstJump => jumpsLeft == numberOfJumps - 1;
        private float JumpCompletion => jumpStopwatch.Completion;
        public Vector2 Velocity => rigidbody2D.velocity;

        [HideInInspector] public new Rigidbody2D rigidbody2D;
        public Reanimator reanimator;
        
        public ContactFilter2D contactFilter;
        public ContactPoint2D? groundContact;
        public ContactPoint2D? ceilingContact;
        public ContactPoint2D? wallContact;
        private readonly ContactPoint2D[] contacts = new ContactPoint2D[16];
        
        private bool wantsToJump;
        private bool wasOnTheGround;
        private int jumpsLeft;
        
        private void Awake() {
            rigidbody2D = GetComponent<Rigidbody2D>();
            reanimator = GetComponent<Reanimator>();
            
            contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        }
        private void OnEnable() {
            inputReader.FJumpEvent += OnJump;
            SasukeStateSO.OnStateTransition += TransitionToState;
        }

        private void OnDisable() {
            inputReader.FJumpEvent += OnJump;
            SasukeStateSO.OnStateTransition -= TransitionToState;
        }
        private void Update() {
            reanimator.Flip = FacingDirection < 0;
        }
        private void FixedUpdate() {
            FindContacts();
            UpdateMovement();
            UpdateDirection();
        }

        private void OnJump(float value) {
            wantsToJump = value > 0.5f;

            if (wantsToJump)
                RequestJump();
            else
                jumpStopwatch.Reset();
        }
        
        private void TransitionToState(SasukeStateSO nextState) {
            if (nextState == remainState)
                return;

            currentState.OnStateExit(this);
            currentState = nextState;
            currentState.OnStateEnter(this);
        }
        private void RequestJump()
        {
            if (jumpsLeft <= 0)
                return;

            jumpsLeft--;
            jumpStopwatch.Split();
        }
        private void UpdateMovement()
        {
            var previousVelocity = rigidbody2D.velocity;
            var velocityChange = Vector2.zero;

            if (wantsToJump && IsJumping)
            {
                wasOnTheGround = false;
                float currentJumpSpeed = IsFirstJump ? firstJumpSpeed : jumpSpeed;
                currentJumpSpeed *= jumpFallOff.Evaluate(JumpCompletion);
                velocityChange.y = currentJumpSpeed - previousVelocity.y;

                if (ceilingContact.HasValue)
                    jumpStopwatch.Reset();
            }
            else if (groundContact.HasValue)
            {
                jumpsLeft = numberOfJumps;
                wasOnTheGround = true;
            }
            else
            {
                if (wasOnTheGround)
                {
                    jumpsLeft -= 1;
                    wasOnTheGround = false;
                }

                velocityChange.y = (-fallSpeed - previousVelocity.y) / 8;
            }

            velocityChange.x = (playerInputData.MovementInput.x * walkSpeed - previousVelocity.x) / 4;

            if (wallContact.HasValue)
            {
                var wallDirection = (int) Mathf.Sign(wallContact.Value.point.x - transform.position.x);
                var walkDirection = (int) Mathf.Sign(playerInputData.MovementInput.x);

                if (walkDirection == wallDirection)
                    velocityChange.x = 0;
            }

            rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
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
        private void UpdateDirection() {
            if (playerInputData.MovementInput.x > 0)
                FacingDirection = 1;
            else if (playerInputData.MovementInput.x < 0)
                FacingDirection = -1;
        }

    }
    
}