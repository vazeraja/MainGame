﻿using System.Collections.Generic;
using Aarthificial.Reanimation;
using UnityEngine;

namespace MainGame {
    public class SasukeRenderer : MonoBehaviour {
        private static class Drivers {
            public const string AttackCompletion = "attackCompletion";
            public const string FlipCompletion = "flipCompletion";
            public const string HitDirection = "hitDirection";
            public const string IsGrounded = "isGrounded";
            public const string IsMoving = "isMoving";
            public const string JumpDirection = "jumpDirection";
            public const string ShouldFlip = "shouldFlip";
            public const string State = "state";
        }

        private Reanimator reanimator;
        private SasukeController controller;
        private CollisionInfo collisionInfo;

        private bool _isRed;

        private void Awake() {
            reanimator = GetComponent<Reanimator>();
            controller = GetComponent<SasukeController>();
            collisionInfo = GetComponent<CollisionInfo>();
        }

        private void OnEnable() {
            reanimator.Ticked += UpdateColor;
        }

        private void OnDisable() {
            reanimator.Ticked -= UpdateColor;
        }

        private void Update() {
            var velocity = collisionInfo.Velocity;
            bool isMoving = Mathf.Abs(controller.DesiredDirection.x) > 0 && Mathf.Abs(velocity.x) > 0.01f;

            int hitDirection;
            float speed = velocity.magnitude;
            var velocityDirection = velocity / speed;
            if (speed < 0.1f || velocityDirection.y < -0.65f)
                hitDirection = 2;
            else if (velocityDirection.y > 0.65f)
                hitDirection = 1;
            else
                hitDirection = 0;

            reanimator.Flip = controller.FacingDirection < 0;
            reanimator.Set(Drivers.State, (int) controller.State);
            reanimator.Set(Drivers.IsGrounded, collisionInfo.IsGrounded);
            reanimator.Set(Drivers.IsMoving, isMoving);
            reanimator.Set(Drivers.JumpDirection, velocity.y > 0);
            reanimator.Set(Drivers.ShouldFlip, controller.IsJumping && !controller.IsFirstJump);
            reanimator.Set(Drivers.FlipCompletion, controller.JumpCompletion);
            reanimator.Set(Drivers.AttackCompletion, controller.AttackCompletion);
            reanimator.Set(Drivers.HitDirection, hitDirection);

            bool didLandInThisFrame = reanimator.WillChange(Drivers.IsGrounded, true);
            bool didDashInThisFrame = reanimator.WillChange(Drivers.State, (int) SasukeState.Dash);

            if (didLandInThisFrame || didDashInThisFrame)
                reanimator.ForceRerender();
        }

        private void UpdateColor() {
            if (controller.State == SasukeState.Hit) {
                reanimator.Renderer.color = _isRed ? Color.red : Color.white;
                _isRed = !_isRed;
            }
            else {
                reanimator.Renderer.color = Color.white;
                _isRed = true;
            }
        }
    }
}