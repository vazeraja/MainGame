using UnityEngine;


public class HitState : State {
    public override void Enter() {
        var relativePosition =
            (Vector2) player.transform.InverseTransformPoint(player.collisionData.transform.position);
        var direction = (player.CollisionDetection.rigidbody2D.centerOfMass - relativePosition).normalized;

        player.hitStopwatch.Split();
        player.CollisionDetection.rigidbody2D.AddForce(
            direction * player.hitForce - player.CollisionDetection.Velocity,
            ForceMode2D.Impulse
        );
    }

    public override void Update() { }

    public override void FixedUpdate() {
        player.FacingDirection = player.CollisionDetection.rigidbody2D.velocity.x < 0 ? -1 : 1;

        player.CollisionDetection.rigidbody2D.AddForce(Physics2D.gravity * 4);
        if (player.hitStopwatch.IsFinished &&
            (player.CollisionDetection.IsGrounded || player.CollisionDetection.IsTouchingWall)) {
            player.hitStopwatch.Split();
            player.EnterMovementState();
        }
    }

    public override void Exit() { }
}