using System.Collections.Generic;
using Aarthificial.Reanimation;
using UnityEngine;

namespace MainGame {
    public class SasukeRenderer : MonoBehaviour {
        private static class Drivers
        {
            public const string AttackCompletion = "attackCompletion";
            public const string FlipCompletion = "flipCompletion";
            public const string HitDirection = "hitDirection";
            public const string IsGrounded = "isGrounded";
            public const string IsMoving = "isMoving";
            public const string JumpDirection = "jumpDirection";
            public const string ShouldFlip = "shouldFlip";
            public const string State = "state";
        }

        private Reanimator _reanimator;
        private SasukeController _controller;

        private bool _isRed;

        private void Awake()
        {
            _reanimator = GetComponent<Reanimator>();
            _controller = GetComponent<SasukeController>();
        }

        private void OnEnable()
        {
            _reanimator.Ticked += UpdateColor;
        }

        private void OnDisable()
        {
            _reanimator.Ticked -= UpdateColor;
        }

        private void Update()
        {
            var velocity = _controller.Velocity;
            bool isMoving = Mathf.Abs(_controller.DesiredDirection.x) > 0 && Mathf.Abs(velocity.x) > 0.01f;
            
            int hitDirection;
            float speed = velocity.magnitude;
            var velocityDirection = velocity / speed;
            if (speed < 0.1f || velocityDirection.y < -0.65f)
                hitDirection = 2;
            else if (velocityDirection.y > 0.65f)
                hitDirection = 1;
            else
                hitDirection = 0;
            
            _reanimator.Flip = _controller.FacingDirection < 0;
            _reanimator.Set(Drivers.State, (int) _controller.State);
            _reanimator.Set(Drivers.IsGrounded, _controller.IsGrounded);
            _reanimator.Set(Drivers.IsMoving, isMoving);
            _reanimator.Set(Drivers.JumpDirection, velocity.y > 0);
            _reanimator.Set(Drivers.ShouldFlip, _controller.IsJumping && !_controller.IsFirstJump);
            _reanimator.Set(Drivers.FlipCompletion, _controller.JumpCompletion);
            _reanimator.Set(Drivers.AttackCompletion, _controller.AttackCompletion);
            _reanimator.Set(Drivers.HitDirection, hitDirection);

            bool didLandInThisFrame = _reanimator.WillChange(Drivers.IsGrounded, true);
            bool didDashInThisFrame = _reanimator.WillChange(Drivers.State, (int) SasukeState.Dash);

            if (didLandInThisFrame || didDashInThisFrame)
                _reanimator.ForceRerender();
        }

        private void UpdateColor()
        {
            if (_controller.State == SasukeState.Hit)
            {
                _reanimator.Renderer.color = _isRed ? Color.red : Color.white;
                _isRed = !_isRed;
            }
            else
            {
                _reanimator.Renderer.color = Color.white;
                _isRed = true;
            }
        }
        
    }
}