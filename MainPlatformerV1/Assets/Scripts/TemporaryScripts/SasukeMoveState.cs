using System.Collections;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

[CreateAssetMenu(fileName = "SasukeMoveState", menuName = "PluggableAI/State/Sasuke/SasukeMoveState")]
public class SasukeMoveState : State<SasukeController> {

    [SerializeField] private PlayerInputData playerInputData;
    public override void OnEnter(SasukeController sasuke) { }

    public override void LogicUpdate(SasukeController sasuke) {
        Move(sasuke);
    }

    public override void OnExit(SasukeController sasuke) { }

    private void Move(SasukeController sasuke) {
        var previousVelocity = sasuke.Velocity;
        var velocityChange = Vector2.zero;

        velocityChange.x = (playerInputData.MovementInput.x * sasuke.walkSpeed - previousVelocity.x) / 4;

        if (sasuke.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(sasuke.wallContact.Value.point.x - sasuke.transform.position.x);
            var walkDirection = (int) Mathf.Sign(playerInputData.MovementInput.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        sasuke.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}