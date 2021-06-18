using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PluggableAI/PlayerData")]
    public class PlayerData : ScriptableObject {

        [Header("Base Player Info")]
        public float maxHealth;
        public float currentHealth = 50;
        public float currentScore = 100;

        [Header("Animation")] 
        public bool isAnimationFinished;
        
        [Header("Move State Settings")]
        public float movementSpeed = 7f;
        public float jumpSpeed = 40f;
        public int facingDirection;

        [Header("Attack State Settings")]
        public float attackRate = 2f;

        [Header("Dash State Settings")]
        public float dashTime = 0.2f; // How long do we dash before we leave
        public float dashMaxForce = 150f;
        
        [Header("Interaction Settings")]
        public LayerMask interactableLayer;
        public float rayDistance = 0.5f;

        public void AnimationFinishTriggerTwo() {
            isAnimationFinished = true;
        }

        public void Reset() {
            facingDirection = 1;
        }
        
        
    }
}
