using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class CustomPhysics<T> : MonoBehaviour {

        #region Public Variables
        public Rigidbody2D RB;
        public Animator Anim;
        public SpriteRenderer SR;
        public Vector2 velocity;
        public Vector2 MovementVelocity;
        public bool IsGrounded;

        #endregion

        #region Internal Variables
        public float minGroundNormalY = 0.65f;
        public float gravityModifier = 1f;

        protected Vector2 groundNormal;

        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitbufferList = new List<RaycastHit2D>(16);

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;

        #endregion

        protected virtual void OnEnable() {
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            SR = GetComponent<SpriteRenderer>();
        }
        protected virtual void OnDisable() {

        }
        protected virtual void Awake() {

        }

        protected virtual void Start() {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // Use settings from Physics2D to determine what layers to check collisions against
            contactFilter.useLayerMask = true;
        }
        protected virtual void Update() {
            MovementVelocity = Vector2.zero;
            ApplyVelocity();
        }
        protected virtual void FixedUpdate() {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            velocity.x = MovementVelocity.x;

            IsGrounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime;
            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);
        }

        void Movement(Vector2 move, bool yMovement) {
            float distance = move.magnitude;

            if (distance > minMoveDistance) {
                int count = RB.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

                hitbufferList.Clear();
                for (int i = 0; i < count; i++) {
                    hitbufferList.Add(hitBuffer[i]);
                }


                foreach (RaycastHit2D hitbuffer in hitbufferList) {
                    Vector2 currentNormal = hitbuffer.normal;
                    if (currentNormal.y > minGroundNormalY) {
                        IsGrounded = true;
                        if (yMovement) {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0) {
                        velocity = velocity - projection * currentNormal;
                    }
                    float modifiedDistance = hitbuffer.distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            RB.position = RB.position + move.normalized * distance;
        }
        protected virtual void ApplyVelocity() {
        }

        public abstract void TransitionToState(T nextState);

    }
}
