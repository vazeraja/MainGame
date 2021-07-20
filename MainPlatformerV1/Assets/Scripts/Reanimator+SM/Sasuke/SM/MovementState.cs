using ThunderNut.StateMachine;
using UnityEngine;


public class MovementState : State {
    
    public PlayerController player => agent as PlayerController;
    
    public override void Enter() {
        player.State = SasukeState.Movement;
    }

    public override void Update() { }

    public override void FixedUpdate() {
        UpdateMovementState();
    }

    public override void Exit() {
    }
    

    private void UpdateMovementState() {
        var previousVelocity = player.CollisionDetection.Velocity;
        var velocityChange = Vector2.zero;

        if (player.MovementDirection.x > 0)
            player.FacingDirection = 1;
        else if (player.MovementDirection.x < 0)
            player.FacingDirection = -1;

        if (player.wantsToJump && player.IsJumping) {
            player.wasOnTheGround = false;
            float currentJumpSpeed = player.IsFirstJump
                ? player.firstJumpSpeed
                : player.secondJumpSpeed;
            currentJumpSpeed *= player.jumpFallOff.Evaluate(player.JumpCompletion);
            velocityChange.y = currentJumpSpeed - previousVelocity.y;

            if (player.CollisionDetection.ceilingContact.HasValue)
                player.jumpStopwatch.Reset();
        }
        else if (player.CollisionDetection.groundContact.HasValue) {
            player.jumpsLeft = player.numberOfJumps;
            player.wasOnTheGround = true;
            player.canDash = true;
        }
        else {
            if (player.wasOnTheGround) {
                player.jumpsLeft -= 1;
                player.wasOnTheGround = false;
            }

            velocityChange.y = (-player.fallSpeed - previousVelocity.y) / 8;
        }

        velocityChange.x = (player.MovementDirection.x * player.walkSpeed - previousVelocity.x) / 4;

        if (player.CollisionDetection.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(player.CollisionDetection.wallContact.Value.point.x -
                                                 player.transform.position.x);
            var walkDirection = (int) Mathf.Sign(player.MovementDirection.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        player.CollisionDetection.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}