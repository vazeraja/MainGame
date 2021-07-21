using System;
using MainGame;
using TN.Common;
using UnityEngine;

namespace Jeff
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JeffController : MonoBehaviour {
        [SerializeField] private InputProvider inputProvider;
        
        public event Action HatTriggered;
        public event Action HitStateEntered;

        [Header("Walking")]
        [SerializeField] private float walkSpeed = 7;

        [Header("Jumping")] 
        [SerializeField] private float firstJumpSpeed = 8;
        [SerializeField] private float jumpSpeed = 3;
        [SerializeField] private float fallSpeed = 12;
        [SerializeField] private int numberOfJumps = 2;
        [SerializeField] private AnimationCurve jumpFallOff = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] private FixedStopwatch jumpStopwatch = new FixedStopwatch();

        [Header("Getting a whooping")] 
        [SerializeField] private Vector2 bounceBackStrength = new Vector2(8, 12);
        [SerializeField] private FixedStopwatch hitStopwatch = new FixedStopwatch();

        [Header("Giving a whooping")] 
        [SerializeField] private float attackSpeed = 12;
        [SerializeField] private FixedStopwatch attackStopwatch = new FixedStopwatch();

        private CollisionDetection collisions;
        public JeffState State { get; private set; } = JeffState.Movement;
        public Vector2 DesiredDirection { get; private set; }
        public int FacingDirection { get; private set; } = 1;

        public Rigidbody2D RB => collisions.rigidbody2D;
        
        public Vector2 Velocity => collisions.rigidbody2D.velocity;
        public float AttackCompletion => attackStopwatch.Completion;
        public float JumpCompletion => jumpStopwatch.Completion;
        public bool IsJumping => !jumpStopwatch.IsFinished;
        public bool IsFirstJump => _jumpsLeft == numberOfJumps - 1;
        
        private bool _wantsToJump;
        private bool _wasOnTheGround;
        private bool _canAttack;
        private int _jumpsLeft;
        private int _enemyLayer;

        private void Awake() {
            collisions = GetComponent<CollisionDetection>();
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void OnEnable() {
            // inputProvider.MoveEvent += OnMove;
            inputProvider.onJump += OnJump;
            inputProvider.onDash += OnAttack;
            inputProvider.HatEvent += OnHat;
        }

        private void OnDisable() {
            // inputProvider.MoveEvent -= OnMove;
            inputProvider.onJump -= OnJump;
            inputProvider.onDash -= OnAttack;
            inputProvider.HatEvent -= OnHat;
        }

        #region Events

        private void OnMove(Vector2 value) => DesiredDirection = value;

        private void OnJump(float value) {
            _wantsToJump = value > 0.5f;
            
            if (_wantsToJump)
                RequestJump();
            else
                jumpStopwatch.Reset();
        }

        private void OnAttack(float value) => EnterAttackState();
        private void OnHat() => HatTriggered?.Invoke();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != _enemyLayer ) return;
            EnterHitState(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.layer != _enemyLayer || State == JeffState.Hit) return;
            EnterHitState(other);
        }

        #endregion

        private void RequestJump()
        {
            if (State != JeffState.Movement || _jumpsLeft <= 0)
                return;

            _jumpsLeft--;
            jumpStopwatch.Split();
        }

        private void FixedUpdate()
        {
            switch (State)
            {
                case JeffState.Movement:
                    UpdateMovementState();
                    break;
                case JeffState.Attack:
                    UpdateAttackState();
                    break;
                case JeffState.Hit:
                    UpdateHitState();
                    break;
            }
        }

        #region States

        private void EnterHitState(Collision2D collision)
        {
            if (State != JeffState.Hit && !hitStopwatch.IsReady) return;
            State = JeffState.Hit;
            HitStateEntered?.Invoke();

            var relativePosition = (Vector2) transform.InverseTransformPoint(collision.transform.position);
            var direction = (RB.centerOfMass - relativePosition).normalized;

            hitStopwatch.Split();
            RB.AddForce(
                direction * bounceBackStrength - RB.velocity,
                ForceMode2D.Impulse
            );
        }

        private void UpdateHitState()
        {
            FacingDirection = RB.velocity.x < 0 ? -1 : 1;

            RB.AddForce(Physics2D.gravity * 4);
            if (hitStopwatch.IsFinished && (collisions.IsGrounded || collisions.IsTouchingWall))
            {
                hitStopwatch.Split();
                EnterMovementState();
            }
        }

        private void EnterAttackState()
        {
            if (State != JeffState.Movement || !attackStopwatch.IsReady || !_canAttack) return;
            State = JeffState.Attack;

            attackStopwatch.Split();
            _canAttack = false;
        }

        private void UpdateAttackState()
        {
            RB.AddForce(
                new Vector2(FacingDirection * attackSpeed, 0) - RB.velocity,
                ForceMode2D.Impulse
            );
            if (attackStopwatch.IsFinished || collisions.IsTouchingWall)
            {
                attackStopwatch.Split();
                EnterMovementState();
            }
        }

        private void EnterMovementState()
        {
            State = JeffState.Movement;
        }

        private void UpdateMovementState()
        {
            var previousVelocity = RB.velocity;
            var velocityChange = Vector2.zero;

            if (DesiredDirection.x > 0)
                FacingDirection = 1;
            else if (DesiredDirection.x < 0)
                FacingDirection = -1;

            if (_wantsToJump && IsJumping)
            {
                _wasOnTheGround = false;
                float currentJumpSpeed = IsFirstJump ? firstJumpSpeed : jumpSpeed;
                currentJumpSpeed *= jumpFallOff.Evaluate(JumpCompletion);
                velocityChange.y = currentJumpSpeed - previousVelocity.y;

                if (collisions.IsTouchingCeiling)
                    jumpStopwatch.Reset();
            }
            else if (collisions.IsGrounded)
            {
                _jumpsLeft = numberOfJumps;
                _wasOnTheGround = true;
                _canAttack = true;
            }
            else
            {
                if (_wasOnTheGround)
                {
                    _jumpsLeft -= 1;
                    _wasOnTheGround = false;
                }

                velocityChange.y = (-fallSpeed - previousVelocity.y) / 8;
            }

            velocityChange.x = (DesiredDirection.x * walkSpeed - previousVelocity.x) / 4;

            if (collisions.wallContact.HasValue)
            {
                var wallDirection = (int) Mathf.Sign(collisions.wallContact.Value.point.x - transform.position.x);
                var walkDirection = (int) Mathf.Sign(DesiredDirection.x);

                if (walkDirection == wallDirection)
                    velocityChange.x = 0;
            }

            RB.AddForce(velocityChange, ForceMode2D.Impulse);
        }

        #endregion
    }
}