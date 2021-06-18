using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "SasukeHitState", menuName = "PluggableAI/State/Sasuke/SasukeHitState")]
    public class SasukeHitState : State<SasukeController> {
        public override void OnEnter(SasukeController sasuke) {
            var relativePosition = (Vector2) sasuke.transform.InverseTransformPoint(sasuke.CollisionData.transform.position);
            var direction = (sasuke.CollisionInfo.rigidbody2D.centerOfMass - relativePosition).normalized;

            sasuke.hitStopwatch.Split();
            sasuke.CollisionInfo.rigidbody2D.AddForce(
                direction * sasuke.hitForce - sasuke.CollisionInfo.Velocity,
                ForceMode2D.Impulse
            );
        }

        public override void LogicUpdate(SasukeController sasuke) {
        }

        public override void PhysicsUpdate(SasukeController sasuke) {
            sasuke.FacingDirection = sasuke.CollisionInfo.rigidbody2D.velocity.x < 0 ? -1 : 1;

            sasuke.CollisionInfo.rigidbody2D.AddForce(Physics2D.gravity * 4);
            if (sasuke.hitStopwatch.IsFinished && (sasuke.CollisionInfo.IsGrounded || sasuke.CollisionInfo.IsTouchingWall))
            {
                sasuke.hitStopwatch.Split();
                sasuke.EnterMovementState();
            }
        }

        public override void OnExit(SasukeController sasuke) {
        }
    }
}