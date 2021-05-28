using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/DashState")]
    public class DashState : State<MainPlayer> {

        public bool isAbilityDone;

        private float holdDownStartTime;
        private bool timerIsRunning = false;
        private float dashTime;
        private float force;

        private static readonly int DashX = Animator.StringToHash("dashX");

        public override void OnEnter(MainPlayer mainPlayer){
            isAbilityDone = false;
            dashTime = mainPlayer.PlayerData.dashTime;
        }
        public override void LogicUpdate(MainPlayer mainPlayer){
            if (mainPlayer.DashInput) {
                holdDownStartTime = Time.time;
                mainPlayer.Anim.SetBool(DashX, false);
            }
            if (!mainPlayer.DashInput) {
                timerIsRunning = true;
                float holdDowntime = Time.time - holdDownStartTime;
                force = CalculateForce(mainPlayer, holdDowntime);

                mainPlayer.Anim.SetBool(DashX, true);
                mainPlayer.MovementVelocity.x = mainPlayer.DashKeyboardInput.x != 0 ? mainPlayer.DashKeyboardInput.x * force : mainPlayer.FacingDirection * force;
                CheckIfShouldFlip(mainPlayer, (int)mainPlayer.DashKeyboardInput.x);
                CheckDashTime();
            }
        }
        public override void OnExit(MainPlayer mainPlayer){
            mainPlayer.Anim.SetBool(DashX, false);
        }
        private void CheckIfShouldFlip(MainPlayer mainPlayer, int input){
            if (input != 0 && input != mainPlayer.FacingDirection)
                mainPlayer.Flip();
        }
        private float CalculateForce(MainPlayer mainPlayer, float holdTime){
            float maxForceHoldDownTime = .25f;
            float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDownTime);
            float force = holdTimeNormalized * mainPlayer.PlayerData.dashMaxForce;
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
