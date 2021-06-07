using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
    public class PlayerData : ScriptableObject {

        [Header("Base Player Info")]
        public float maxHealth;
        public float currentHealth = 50;
        public float currentScore = 100;

        [Header("Move State Variables")]
        public float movementSpeed = 7f;
        public float jumpSpeed = 40f;

        [Header("Attack State Variables")]
        public float attackRate = 2f;

        [Header("Dash State Variables")]
        public float dashTime = 0.2f; // How long do we dash before we leave
        public float dashMaxForce = 150f;

        public LayerMask interactable;
        public float interactableDistance = 0.5f;
    }
}
