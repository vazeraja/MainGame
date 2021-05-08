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
        private bool dashInputStop;
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
        }


        public override void OnExit(Player player) {
        }
    }

}
