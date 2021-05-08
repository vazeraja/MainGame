using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame {
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data/Base Data")]
    public class PlayerData : ScriptableObject {

        public float movementSpeed = 7f;
        public float jumpSpeed = 10f;

        public float attackRate = 2f;

        public float lookSphereCastRadius = 3f;

        [Header("Dash State")]
        public float dashCoolDown = 0.5f;
        public float dashMaxHoldTime = 5f;
        [Tooltip("Slows down time during dash if enabled.")] public float holdTimeScale = 0.25f;
        [Tooltip("Duration of the dash animation.")] public float dashTime = 0.2f;
        public float dashVelocity = 30f;
        [Tooltip("Changes drag field in rigidybody, slows down player.")] public float drag = 10f;
        public float variableDashMultiplier = 0.2f;
        public float distanceBetweenAfterImages = 0.5f;
        [Tooltip("Toggles time freeze during dash.")] public bool dashTimeFreeze;

    }
}


