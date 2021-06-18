using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "SasukeMovementState", menuName = "PluggableAI/State/Sasuke/SasukeMovementState")]
public class SasukeMovementState : State<SasukeController> {
    [SerializeField] private PlayerInputData playerInputData;

    public override void OnEnter(SasukeController sasuke) {
        sasuke.State = SasukeState.Movement;
    }

    public override void LogicUpdate(SasukeController sasuke) {
    }

    public override void PhysicsUpdate(SasukeController sasuke) {
        UpdateMovementState(sasuke);
    }

    public override void OnExit(SasukeController sasuke) { }

    private void UpdateMovementState(SasukeController sasukeController) {
        var previousVelocity = sasukeController.rigidbody2D.velocity;
        var velocityChange = Vector2.zero;

        if (sasukeController.DesiredDirection.x > 0)
            sasukeController.FacingDirection = 1;
        else if (sasukeController.DesiredDirection.x < 0)
            sasukeController.FacingDirection = -1;

        if (sasukeController.wantsToJump && sasukeController.IsJumping) {
            sasukeController.wasOnTheGround = false;
            float currentJumpSpeed = sasukeController.IsFirstJump ? sasukeController.firstJumpSpeed : sasukeController.jumpSpeed;
            currentJumpSpeed *= sasukeController.jumpFallOff.Evaluate(sasukeController.JumpCompletion);
            velocityChange.y = currentJumpSpeed - previousVelocity.y;

            if (sasukeController.ceilingContact.HasValue)
                sasukeController.jumpStopwatch.Reset();
        }
        else if (sasukeController.groundContact.HasValue) {
            sasukeController.jumpsLeft = sasukeController.numberOfJumps;
            sasukeController.wasOnTheGround = true;
            sasukeController.canDash = true;
        }
        else {
            if (sasukeController.wasOnTheGround) {
                sasukeController.jumpsLeft -= 1;
                sasukeController.wasOnTheGround = false;
            }

            velocityChange.y = (-sasukeController.fallSpeed - previousVelocity.y) / 8;
        }

        velocityChange.x = (sasukeController.DesiredDirection.x * sasukeController.walkSpeed - previousVelocity.x) / 4;

        if (sasukeController.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(sasukeController.wallContact.Value.point.x - sasukeController.transform.position.x);
            var walkDirection = (int) Mathf.Sign(sasukeController.DesiredDirection.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        sasukeController.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}