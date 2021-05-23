using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/DashState")]
    public class DashState : State<Player> {

        public bool isAbilityDone;

        private float holdDownStartTime;
        private bool timerIsRunning = false;
        private float dashTime;
        private float force;

        private static readonly int DashX = Animator.StringToHash("dashX");

        public override void OnEnter(Player player){
            isAbilityDone = false;
            dashTime = player.PlayerData.dashTime;
        }
        public override void LogicUpdate(Player player){
            if (player.DashInput) {
                holdDownStartTime = Time.time;
                player.Anim.SetBool(DashX, false);
            }
            if (!player.DashInput) {
                timerIsRunning = true;
                float holdDowntime = Time.time - holdDownStartTime;
                force = CalculateForce(player, holdDowntime);

                player.Anim.SetBool(DashX, true);
                player.MovementVelocity.x = player.DashKeyboardInput.x != 0 ? player.DashKeyboardInput.x * force : player.FacingDirection * force;
                CheckIfShouldFlip(player, (int)player.DashKeyboardInput.x);
                CheckDashTime();
            }
        }
        public override void OnExit(Player player){
            player.Anim.SetBool(DashX, false);
        }
        private void CheckIfShouldFlip(Player player, int input){
            if (input != 0 && input != player.FacingDirection)
                player.Flip();
        }
        private float CalculateForce(Player player, float holdTime){
            float maxForceHoldDownTime = .25f;
            float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDownTime);
            float force = holdTimeNormalized * player.PlayerData.dashMaxForce;
            return force;
        }
        private void CheckDashTime(){
            if (timerIsRunning) {
                if (dashTime > 0) {
                    dashTime -= Time.deltaTime;
                }
                else {
                    timerIsRunning = false;
                    dashTime = 0;
                    isAbilityDone = true;
                }
            }
        }
    }

}
