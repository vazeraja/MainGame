using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {

    [CreateAssetMenu(fileName = "attackData", menuName = "Game/Player Data/Attack Data")]
    public class AttackData : ScriptableObject {
        public enum AttackType { Fire, Water, Earth, Air }

        [Header("Attack Info")]
        public string attackName = "";
        public string attackDescription = "";
        public AttackType attackType;
        public float attackDamage;

        [Header("Attack Data")]
        public float currentCooldown = 0f;
        public float maxCooldown = 5f;


        // Factory method. A method that is responsible for creating the object you want with the inputs you like.
        private void Init(float damage, float currentCooldown, float maxCooldown){
            attackDamage = damage;
            this.currentCooldown = currentCooldown;
            this.maxCooldown = maxCooldown;
        }

        public static AttackData CreateInstance(float damage, float currentCooldown, float maxCooldown){
            var data = ScriptableObject.CreateInstance<AttackData>();
            data.Init(damage, currentCooldown, maxCooldown);
            return data;
        }

        public bool IsAttackReady(){
            return currentCooldown <= 0;
        }
    }
}
