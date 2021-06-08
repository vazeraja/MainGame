using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MainGame {
    public abstract class CustomPhysics : MonoBehaviour {

        #region Public Variables
        [HideInInspector] public Rigidbody2D RB;
        [HideInInspector] public Animator Anim;
        [HideInInspector] public SpriteRenderer SR;
        [HideInInspector] public Vector2 velocity;
        [HideInInspector] public Vector2 MovementVelocity;
        public new BoxCollider2D collider;
        public bool IsGrounded => _isGrounded;
        #endregion

        #region Internal Variables
        private const float MINGroundNormalY = 0.65f;
        public float gravityModifier = 1f;

        private Vector2 _groundNormal;
        private bool _isGrounded;

        private ContactFilter2D _contactFilter;
        private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
        private readonly List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

        private const float MINMoveDistance = 0.001f;
        private const float ShellRadius = 0.01f;
        #endregion

        #region Unity Callbacks Functions
        protected virtual void OnEnable(){
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            SR = GetComponent<SpriteRenderer>();
        }
        protected virtual void Start(){
            _contactFilter.useTriggers = false;
            _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // Use settings from Physics2D to determine what layers to check collisions against
            _contactFilter.useLayerMask = true;
        }
        protected virtual void Update(){
            MovementVelocity = Vector2.zero;
        }
        protected virtual void FixedUpdate(){
            velocity += Physics2D.gravity * (gravityModifier * Time.deltaTime);
            velocity.x = MovementVelocity.x;

            _isGrounded = false;

            var deltaPosition = velocity * Time.deltaTime;
            var moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
            var move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);
        }
        #endregion

        #region Physics Logic
        private void Movement(Vector2 move, bool yMovement){
            float distance = move.magnitude;

            if (distance > MINMoveDistance) {
                int count = RB.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

                _hitBufferList.Clear();
                for (int i = 0; i < count; i++)
                    _hitBufferList.Add(_hitBuffer[i]);


                foreach (var buffer in _hitBufferList) {
                    var currentNormal = buffer.normal;
                    if (currentNormal.y > MINGroundNormalY) {
                        _isGrounded = true;
                        if (yMovement) {
                            _groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                        velocity -= projection * currentNormal;
                    float modifiedDistance = buffer.distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            RB.position += move.normalized * distance;
        }
        #endregion
    }
}
