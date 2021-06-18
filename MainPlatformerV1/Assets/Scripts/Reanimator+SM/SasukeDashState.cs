using UnityEngine;

namespace MainGame {
    
    [CreateAssetMenu(fileName = "SasukeDashState", menuName = "PluggableAI/State/Sasuke/SasukeDashState")]
    public class SasukeDashState : State<SasukeController> {
        public override void OnEnter(SasukeController sasuke) {
            sasuke.dashStopwatch.Split();
            sasuke.canDash = false;
        }

        public override void LogicUpdate(SasukeController sasuke) {
        }

        public override void PhysicsUpdate(SasukeController sasuke) {
            sasuke.CollisionInfo.rigidbody2D.AddForce(
                new Vector2(sasuke.FacingDirection * sasuke.dashSpeed, 0) - sasuke.CollisionInfo.rigidbody2D.velocity,
                ForceMode2D.Impulse
            );
            if (!sasuke.dashStopwatch.IsFinished && !sasuke.CollisionInfo.IsTouchingWall) return;
            sasuke.dashStopwatch.Split();
            sasuke.EnterMovementState();
            
        }

        public override void OnExit(SasukeController sasuke) {
            
        }
    }
}