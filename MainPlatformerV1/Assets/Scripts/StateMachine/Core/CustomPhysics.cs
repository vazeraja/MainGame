﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public abstract class CustomPhysics : MonoBehaviour {

        #region Public Variables
        [HideInInspector] public Rigidbody2D RB;
        [HideInInspector] public Animator Anim;
        [HideInInspector] public SpriteRenderer SR;
        [HideInInspector] public Vector2 velocity;
        [HideInInspector] public Vector2 MovementVelocity;
        [HideInInspector] public bool IsGrounded;
        #endregion

        #region Internal Variables
        public float minGroundNormalY = 0.65f;
        public float gravityModifier = 1f;

        private Vector2 groundNormal;

        private ContactFilter2D contactFilter;
        private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        private List<RaycastHit2D> hitbufferList = new List<RaycastHit2D>(16);

        private const float minMoveDistance = 0.001f;
        private const float shellRadius = 0.01f;
        #endregion

        #region Unity Callbacks Functions

        protected virtual void OnEnable(){
            RB = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            SR = GetComponent<SpriteRenderer>();
        }
        protected virtual void OnDisable(){

        }
        protected virtual void Awake(){

        }

        protected virtual void Start(){
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // Use settings from Physics2D to determine what layers to check collisions against
            contactFilter.useLayerMask = true;
        }
        protected virtual void Update(){
            MovementVelocity = Vector2.zero;
        }
        protected virtual void FixedUpdate(){
            velocity += Physics2D.gravity * (gravityModifier * Time.deltaTime);
            velocity.x = MovementVelocity.x;

            IsGrounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime;
            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);
        }
        
        #endregion

        #region Physics Logic
        private void Movement(Vector2 move, bool yMovement){
            float distance = move.magnitude;

            if (distance > minMoveDistance) {
                int count = RB.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

                hitbufferList.Clear();
                for (int i = 0; i < count; i++) {
                    hitbufferList.Add(hitBuffer[i]);
                }


                foreach (var hitBuffer in hitbufferList) {
                    var currentNormal = hitBuffer.normal;
                    if (currentNormal.y > minGroundNormalY) {
                        IsGrounded = true;
                        if (yMovement) {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    float projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0) {
                        velocity -= projection * currentNormal;
                    }
                    float modifiedDistance = hitBuffer.distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            RB.position += move.normalized * distance;
        }
        
        #endregion
    }
}
