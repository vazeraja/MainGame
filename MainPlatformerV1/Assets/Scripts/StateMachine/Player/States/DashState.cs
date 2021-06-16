using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/DashState")]
    public class DashState : State<Player> {
        [SerializeField] private PlayerInputData playerInputData;
        [SerializeField] private PlayerData playerData;

        public bool isAbilityDone;

        private float holdDownStartTime;
        private bool timerIsRunning = false;
        private float dashTime;
        private float force;

        private static readonly int DashX = Animator.StringToHash("dashX");

        public override void OnEnter(Player player){
            isAbilityDone = false;
            dashTime = playerData.dashTime;
        }
        public override void LogicUpdate(Player player){
            if (playerInputData.DashInput) {
                holdDownStartTime = Time.time;
                player.Anim.SetBool(DashX, false);
            }
            if (!playerInputData.DashInput) {
                timerIsRunning = true;
                float holdDowntime = Time.time - holdDownStartTime;
                force = CalculateForce(holdDowntime);

                player.Anim.SetBool(DashX, true);
                player.MovementVelocity.x = playerInputData.DashKeyboardInput.x != 0 
                    ? playerInputData.DashKeyboardInput.x * force : playerData.facingDirection * force;
                CheckIfShouldFlip(player, (int)playerInputData.DashKeyboardInput.x);
                CheckDashTime();
            }
        }
        public override void OnExit(Player player){
            player.Anim.SetBool(DashX, false);
        }
        private void CheckIfShouldFlip(Player player, int input) {
            if (input == 0 || input == playerData.facingDirection) return;
            playerData.facingDirection *= -1;
            player.transform.Rotate(0f, -180f, 0f);
        }
        private float CalculateForce(float holdTime){
            float maxForceHoldDownTime = .25f;
            float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDownTime);
            float force = holdTimeNormalized * playerData.dashMaxForce;
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
