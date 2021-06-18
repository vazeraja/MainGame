using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "SasukeHitState", menuName = "PluggableAI/State/Sasuke/SasukeHitState")]
    public class SasukeHitState : State<SasukeController> {
        public override void OnEnter(SasukeController sasuke) {
            var relativePosition = (Vector2) sasuke.transform.InverseTransformPoint(sasuke.CollisionData.transform.position);
            var direction = (sasuke.rigidbody2D.centerOfMass - relativePosition).normalized;

            sasuke.hitStopwatch.Split();
            sasuke.rigidbody2D.AddForce(
                direction * sasuke.hitForce - sasuke.rigidbody2D.velocity,
                ForceMode2D.Impulse
            );
        }

        public override void LogicUpdate(SasukeController sasuke) {
        }

        public override void PhysicsUpdate(SasukeController sasuke) {
            sasuke.FacingDirection = sasuke.rigidbody2D.velocity.x < 0 ? -1 : 1;

            sasuke.rigidbody2D.AddForce(Physics2D.gravity * 4);
            if (sasuke.hitStopwatch.IsFinished && (sasuke.groundContact.HasValue || sasuke.wallContact.HasValue))
            {
                sasuke.hitStopwatch.Split();
                sasuke.EnterMovementState();
            }
        }

        public override void OnExit(SasukeController sasuke) {
        }
    }
}