using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainGame {
    public enum AttackType { SaberGreen = 0, SaberOrange = 1, SaberPurple = 2 }

    [Serializable]
    public class Attack {
        public string name;
        public float length;
    }

    [Serializable]
    public class ComboInput {
        public AttackType type;

        public ComboInput(AttackType t){
            type = t;
        }

        public bool IsSameAs(ComboInput test){
            return (type == test.type);
        }
    }

    [Serializable]
    public class Combo {
        public string name;
        public List<ComboInput> inputs;
        public Attack comboAttack;
        public UnityEvent onCombo;
        int curInput = 0;

        public bool ContinueCombo(ComboInput i){
            if (inputs[curInput].IsSameAs(i)) {
                curInput++;
                if (curInput >= inputs.Count) {
                    onCombo?.Invoke();
                    curInput = 0;
                }
                return true;
            }
            else {
                curInput = 0;
                return false;
            }
        }

        public ComboInput CurrentComboInput(){
            if (curInput >= inputs.Count) return null;
            return inputs[curInput];
        }
        public void ResetCombo(){
            curInput = 0;
        }
    }

}
