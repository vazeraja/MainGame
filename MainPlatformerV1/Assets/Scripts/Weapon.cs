using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    public class Weapon : MonoBehaviour {

        private MainPlayer controller;

        [HideInInspector] public Animator baseAnimator;
        [HideInInspector] public Animator weaponAnimator;

        private void OnEnable(){
            controller = GetComponentInParent<MainPlayer>();

            baseAnimator = transform.Find("Base").GetComponent<Animator>();
            weaponAnimator = transform.Find("Attack").GetComponent<Animator>();
        }
        public void EnterWeapon(){
            gameObject.SetActive(true);

            // Resets attack counter after final attack sequence reached, Reset counter once time since first attack passes the reset time limit
            // if (attackCounter >= weaponData.movementSpeed.Length || Time.time - timeSinceFirstAttack >= weaponData.resetTime) {
            //     attackCounter = 0;
            // }

            // Keep track of time when first attack is used
            // if (attackCounter == 0) timeSinceFirstAttack = Time.time;

            // baseAnimator.SetBool("attack", true);
            // weaponAnimator.SetBool("attack", true);

            // baseAnimator.SetInteger("attackCounter", attackCounter);
            // weaponAnimator.SetInteger("attackCounter", attackCounter);

            controller.SR.enabled = false;
            baseAnimator.SetBool("attack", true);
            weaponAnimator.SetBool("attack", true);
        }
        public void ExitWeapon(){
            baseAnimator.SetBool("attack", false);
            weaponAnimator.SetBool("attack", false);
            controller.SR.enabled = true;

            gameObject.SetActive(false);
        }

        public void AnimationFinishTrigger(){
            controller.AnimationFinishTrigger();
        }

    }
}
