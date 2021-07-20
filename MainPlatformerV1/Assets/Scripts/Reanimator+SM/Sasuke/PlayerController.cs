using System;
using UnityEngine;
using UnityEngine.Serialization;
using ThunderNut.StateMachine;
using TN.Common;
using TN.GameEngine;

public enum SasukeState {
    Movement = 0,
    Dash = 1,
    Hit = 2,
}

public class PlayerController : MonoBehaviour, ISaveable {
    
    [Header("Input")] 
    public InputProvider inputProvider;
    
    [Header("Walking")] 
    public float walkSpeed = 7;

    [Header("Jumping")] 
    public float firstJumpSpeed;
    public float secondJumpSpeed;
    public float fallSpeed;
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
    
    private RuntimeStateMachine stateMachine;
    public CollisionDetection CollisionDetection { get; set; }
    public Vector2 MovementDirection { get; set; }
    public bool IsCrouching { get; set; }
    public int FacingDirection { get; set; } = 1;
    public SasukeState State { get; set; } = SasukeState.Movement;

    public float DashCompletion => dashStopwatch.Completion;
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

        RuntimeStateMachine stateMachine = Builder.RuntimeStateMachine
            .WithState(new MovementState(), out var movementState)
            .WithState(new DashState(), out var dashState)
            .WithState(new HitState(), out var hitState)
            .WithState(new RemainState(), out var remainState)
            .WithTransition(new Transition(), new EnterMovementStateDecision(), movementState, remainState,
                new[] {dashState, hitState})
            .WithTransition(new Transition(), new EnterDashStateDecision(), dashState, remainState, 
                new[] {movementState})
            .WithTransition(new Transition(), new EnterHitStateDecision(), hitState, remainState,
                new[] {movementState, dashState})
            .SetCurrentState(movementState)
            .SetRemainState(remainState);
    }
    private void OnEnable() {
        inputProvider.MoveEvent += OnMove;
        inputProvider.JumpEvent += OnJump;
        inputProvider.DashEvent += OnDash;
        inputProvider.CrouchEvent += OnCrouch;
    }

    private void OnDisable() {
        inputProvider.MoveEvent -= OnMove;
        inputProvider.JumpEvent -= OnJump;
        inputProvider.CrouchEvent -= OnCrouch;
        inputProvider.DashEvent -= OnDash;
    }
    
    private void Start() {
        JsonSaveService.LoadJsonData(new[] {this});
        enemyLayer = LayerMask.NameToLayer($"Enemy");
        stateMachine.Bind<PlayerController>(this);
    }

    private void Update() {
        Debug.Log(inputProvider.GetState().movementDirection);
        stateMachine.Update();
    } 

    private void FixedUpdate() => stateMachine.FixedUpdate();

    private void OnApplicationQuit() {
        JsonSaveService.SaveJsonData(new []{this});
    }

    #region Events

    private void OnMove(Vector2 value) => MovementDirection = value;

    private void OnJump(float value) {
        wantsToJump = value > 0.5f;
        HandleJump();

        void HandleJump() {
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
    }

    private void OnDash(float value) => EnterDashState();

    private void OnCrouch(float value) {
        IsCrouching = value > 0.5f;
    }

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

    public void PopulateSaveData(SaveData saveData) {
        saveData.profile.location = transform.position;
    }

    public void LoadFromSaveData(SaveData saveData) {
        transform.position = saveData.profile.location;
    }
}