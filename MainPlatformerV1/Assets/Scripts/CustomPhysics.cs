using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public class CustomPhysics : MonoBehaviour {

        public float minGroundNormalY = 0.65f;
        public float gravityModifier = 1f;

        protected Vector2 targetVelocity;
        protected bool grounded;
        protected Vector2 groundNormal;

        protected Vector2 velocity;
        protected Rigidbody2D rb2d;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitbufferList = new List<RaycastHit2D>(16);

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;

        protected virtual void OnEnable() {
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
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
            targetVelocity = Vector2.zero;
            ApplyVelocity();
        }
        protected virtual void FixedUpdate() {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            velocity.x = targetVelocity.x;

            grounded = false;

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
                int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

                hitbufferList.Clear();
                for (int i = 0; i < count; i++) {
                    hitbufferList.Add(hitBuffer[i]);
                }

                for (int i = 0; i < hitbufferList.Count; i++) {
                    Vector2 currentNormal = hitbufferList[i].normal;
                    if (currentNormal.y > minGroundNormalY) {
                        grounded = true;
                        if (yMovement) {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0) {
                        velocity = velocity - projection * currentNormal;
                    }
                    float modifiedDistance = hitbufferList[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            rb2d.position = rb2d.position + move.normalized * distance;
        }
        protected virtual void ApplyVelocity() {
        }

    }
}
