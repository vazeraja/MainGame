using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;


public enum SasukeState {
    Movement = 0,
    Dash = 1,
    Hit = 2,
}

public class SasukeController : MonoBehaviour {
    [Header("Input")] 
    [SerializeField] private InputReader inputReader;
    
    #region Variables

    [Header("Walking")] 
    public float walkSpeed = 7;

    [Header("Jumping")] 
    public float firstJumpSpeed = 8;
    [FormerlySerializedAs("jumpSpeed")] public float secondJumpSpeed = 3;
    public float fallSpeed = 12;
    public int numberOfJumps = 2;
    public AnimationCurve jumpFallOff = AnimationCurve.Linear(0, 1, 1, 0);
    public FixedStopwatch jumpStopwatch = new FixedStopwatch();

    [Header("Getting a whooping")] 
    public Vector2 hitForce;
    public FixedStopwatch hitStopwatch = new FixedStopwatch();
    public Collision2D collisionData;

    [Header("Giving a whooping")] 
    public float dashSpeed = 12;
    public FixedStopwatch dashStopwatch = new FixedStopwatch();

    #endregion

    public RuntimeStateMachine StateMachine { get; private set; }
    public CollisionDetection CollisionDetection { get; private set; }
    public SasukeState State { get; set; } = SasukeState.Movement;
    public Vector2 DesiredDirection { get; private set; }
    public int FacingDirection { get; set; } = 1;

    public float AttackCompletion => dashStopwatch.Completion;
    public float JumpCompletion => jumpStopwatch.Completion;
    public bool IsJumping => !jumpStopwatch.IsFinished;
    public bool IsFirstJump => jumpsLeft == numberOfJumps - 1;


    [HideInInspector] public bool wantsToJump;
    [HideInInspector] public bool wasOnTheGround;
    [HideInInspector] public int jumpsLeft;
    [HideInInspector] public bool canDash;
    [HideInInspector] public int enemyLayer;

    private void Awake() {
        CollisionDetection = GetComponent<CollisionDetection>();
    }

    private void Start() {
        enemyLayer = LayerMask.NameToLayer($"Enemy");
    }

    private void OnEnable() {
        inputReader.MoveEvent += OnMove;
        inputReader.FJumpEvent += OnJump;
        inputReader.AttackEvent += OnDash;
    }

    private void OnDisable() {
        inputReader.MoveEvent -= OnMove;
        inputReader.FJumpEvent -= OnJump;
        inputReader.AttackEvent -= OnDash;
    }

    #region Events

    private void OnMove(Vector2 value) => DesiredDirection = value;

    private void OnJump(float value) {
        wantsToJump = value > 0.5f;

        if (wantsToJump) {
            if (State != SasukeState.Movement || jumpsLeft <= 0)
                return;

            jumpsLeft--;
            jumpStopwatch.Split();
        }
        else {
            jumpStopwatch.Reset();
        }
    }

    private void OnDash() => EnterDashState();

    #endregion

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer != enemyLayer) return;
        collisionData = other;
        EnterHitState();
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.layer != enemyLayer || State == SasukeState.Hit) return;
        collisionData = other;
        EnterHitState();
    }

    #region States

    private void EnterHitState() {
        if (State != SasukeState.Hit && !hitStopwatch.IsReady) return;
        State = SasukeState.Hit;
    }

    private void EnterDashState() {
        if (State != SasukeState.Movement || !dashStopwatch.IsReady || !canDash) return;
        State = SasukeState.Dash;
    }

    public void EnterMovementState() {
        State = SasukeState.Movement;
    }

    #endregion
}