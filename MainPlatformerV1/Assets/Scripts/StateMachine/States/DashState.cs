using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/DashState")]
    public class DashState : State {

        public bool CanDash { get; private set; }
        private bool isHolding;

        private float lashDashTime;

        private Vector2 dashDirection;
        private int dashX;
        private int dashY;

        public override void OnEnter(Player player) {

            // If you're in this state, it means you've pressed dash, so you let the state and InputHandler know
            CanDash = false;
            isHolding = true; // If you're in this state, then you are pressing dash, so it is initially true
            dashDirection = Vector2.right * player.FacingDirection;

            if (player.PlayerData.dashTimeFreeze) Time.timeScale = player.PlayerData.holdTimeScale; // Slows down time if its enabled in playerData

            player.startTime = Time.unscaledTime;
        }
        public override void LogicUpdate(Player player) {

            // Set x and y velocity of player
            player.Anim.SetFloat("yVelocity", player.yVelocity);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.MovementVelocity.x));

            // Logic for when Dash button is being held down
            if (isHolding) {

                // ** Set all InputHandler variables **
                dashX = (int)player.DashKeyboardInput.x;
                dashY = (int)player.DashKeyboardInput.y;

                if (!player.DashInput || Time.unscaledTime >= player.startTime + player.PlayerData.dashMaxHoldTime) {
                    ApplyDash(player); // Apply Dash Logic
                }
            } else {
                ApplyDashTimeEnd(player); // Apply Dash if buttoh is held down too long
            }
        }
        public override void OnExit(Player player) {


        }
        private void ApplyDash(Player player) {
            isHolding = false;
            player.startTime = Time.time;
            if (player.PlayerData.dashTimeFreeze) Time.timeScale = 1f; // Enable Time Freeze

            // ** Use this for arrow key dashing **
            dashDirection.x = dashX;
            dashDirection.y = dashY;

            if (dashDirection == Vector2.zero) dashDirection.x = player.FacingDirection;

            if (dashDirection.y != 0)
                player.Anim.SetBool("dashY", true);

            if (dashDirection.x != 0)
                player.Anim.SetBool("dashX", true);

            CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x), player); // Rotate Player based on dash direction
            player.RB.drag = player.PlayerData.drag;
            player.Anim.enabled = true;
            player.MovementVelocity = dashDirection.normalized * player.PlayerData.dashVelocity;
        }
        private void ApplyDashTimeEnd(Player player) {
            player.Anim.enabled = true;
            if (dashDirection.x != 0) player.Anim.SetBool("dashX", true);
            if (dashDirection.y != 0) player.Anim.SetBool("dashY", true);
            player.MovementVelocity = dashDirection.normalized * player.PlayerData.dashVelocity;

            if (Time.time >= player.startTime + player.PlayerData.dashTime) {
                player.RB.drag = 0f;
                lashDashTime = Time.time;
            }
        }
        public void CheckIfShouldFlip(int xInput, Player player) {
            if (xInput != 0 && xInput != player.FacingDirection) {
                Flip(player);
            }
        }
        private void Flip(Player player) {
            player.FacingDirection *= -1;

            var scale = player.transform.localScale;
            scale.x *= -1;
            player.transform.localScale = scale;

        }

        public bool CheckIfCanDash(Player player) => CanDash && Time.time > lashDashTime + player.PlayerData.dashCoolDown;
        public void ResetCanDash() => CanDash = true;
    }

}
