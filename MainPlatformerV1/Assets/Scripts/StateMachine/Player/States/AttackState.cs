using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MainGame {

    [CreateAssetMenu(menuName = "PluggableAI/State/AttackState")]
    public class AttackState : State<Player> {
        
        [SerializeField] private InputActionMap comboMap = new InputActionMap();
        private ReadOnlyArray<InputAction> Actions => comboMap.actions;

        private static readonly int Combo = Animator.StringToHash("combo");
        private static readonly int NoCombo = Animator.StringToHash("noCombo");

        private float timeLeftToCombo = 0;
        private const float comboTime = .25f;
        private int buttonPresses;
        private bool buttonReleased = true;

        private void OnEnable(){
            comboMap.Enable();

            Actions.ForEach(x => x.started += _ => ComboButtonStarted());
            Actions.ForEach(x => x.canceled += _ => ComboButtonReleased());
        }
        private void OnDisable(){
            comboMap.Disable();
        }

        public override void OnEnter(Player player){
            buttonPresses = 1;
            timeLeftToCombo = comboTime;
        }

        public override void LogicUpdate(Player player){
            if (timeLeftToCombo > 0) {
                // Debug.Log(timeLeftToCombo);
                timeLeftToCombo -= Time.deltaTime;
            }
            else
                player.Anim.SetBool(buttonPresses >= 3 ? Combo : NoCombo, true);
        }

        public override void PhysicsUpdate(Player type) {
            
        }

        public override void OnExit(Player player){
            // Debug.Log("Button Presses: " + buttonPresses);

            player.Anim.SetBool(Combo, false);
            player.Anim.SetBool(NoCombo, false);
        }

        private void ComboButtonStarted(){
            if (!buttonReleased) return;
            buttonPresses += 1;
            buttonReleased = false;
        }
        private void ComboButtonReleased(){
            buttonReleased = true;
        }



    }
}
