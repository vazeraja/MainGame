using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/SasukeDashState")]
public class DashState : ActionState {
    public override void Enter() {
        player.dashStopwatch.Split();
        player.canDash = false;
    }

    public override void Update() {
    }

    public override void FixedUpdate() {
        player.CollisionDetection.rigidbody2D.AddForce(
            new Vector2(player.FacingDirection * player.dashSpeed, 0) - player.CollisionDetection.rigidbody2D.velocity,
            ForceMode2D.Impulse
        );
        if (!player.dashStopwatch.IsFinished && !player.CollisionDetection.IsTouchingWall) return;
        player.dashStopwatch.Split();
        player.EnterMovementState();
    }

    public override void Exit() {
    }

}