using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float Speed = 10;
    public float JumpForce = 10;
    public float DistanceToGround = 0.3f;
    public float JumpScheduleTime = 0.1f;
    public int JumpCountLimit = 2;

    private Rigidbody2D rigidBody2D = null;
    private BoxCollider2D playerCollider;
    private GamePhysicsManager physicsManager;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private PlayerStateManager playerStateManager;

    /// <summary>
    /// Whether a jump is being scheduled in the next JumpScheduleTime seconds
    /// When set to true, schedules a jump to be executed if the player lands in JumpScheduleTime time 
    /// </summary>
    public bool JumpScheduled {
        get => lastScheduledJump >= 0 && Time.time - lastScheduledJump < JumpScheduleTime;
        private set => lastScheduledJump = value ? Time.time : -1;
    }
    private float lastScheduledJump = -1;

    public int MidAirJumpsRemaining { get; private set; }

    /// <summary>
    /// Whether the player is on the ground
    /// </summary>
    public bool IsGrounded {
        get => isGrounded;
        private set {
            isGrounded = value;
            playerAnimator?.SetBool("Air", !isGrounded);
        }
    }
    private bool isGrounded = false;

    /// <summary>
    /// Whether the player is walking
    /// </summary>
    public bool IsWalking {
        get => isWalking;
        private set {
            isWalking = value;
            playerAnimator?.SetBool("Run", isWalking);
        }
    }
    private bool isWalking = false;

    void Awake() {
        rigidBody2D = ComponentUtils.GetOrCreate<Rigidbody2D>(gameObject);
        playerCollider = ComponentUtils.Get<BoxCollider2D>(gameObject);
        playerAnimator = ComponentUtils.GetOrNull<Animator>(gameObject);
        spriteRenderer = ComponentUtils.Get<SpriteRenderer>(gameObject);
    }

    void Start() {
        physicsManager = GamePhysicsManager.Instance;
        physicsManager.OnGravityChange += OnGravityChange;
        playerStateManager = PlayerStateManager.Instance;
    }

    void FixedUpdate() {
        HandleScheduledJump();
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        if (IsWalking) {
            spriteRenderer.flipX = inputX < 0;
            var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
            rigidBody2D.velocity = transform.TransformDirection(new Vector2(inputX * Speed, localVelocity.y));
        }
    }

    // Update is called once per frame
    void Update() {
        UpdatePlayerState();
        HandleInput();
    }

    private void HandleInput() {
        if (!playerStateManager.Living) {
            IsWalking = false;
            JumpScheduled = false;
            return;
        }
        if (CrossPlatformInputManager.GetButton("Horizontal")) {
            IsWalking = true;
        } else {
            IsWalking = false;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            JumpScheduled = true;
        }
    }

    private void HandleScheduledJump() {
        if (!JumpScheduled) return;
        if (IsGrounded) {
            Jump();
            MidAirJumpsRemaining = JumpCountLimit - 1;
            JumpScheduled = false;
        } else if (MidAirJumpsRemaining > 0) {
            MidAirJumpsRemaining--;
            Jump();
            JumpScheduled = false;
        }
    }

    private void OnGravityChange(object sender, EventArgs e) {
        if (IsGrounded) {
            // Counts as a jump
            MidAirJumpsRemaining = JumpCountLimit - 1;
        }
    }

    private void Jump() {
        var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
        rigidBody2D.velocity = transform.TransformDirection(new Vector2(localVelocity.x, 0));
        rigidBody2D.AddRelativeForce(new Vector2(0,JumpForce), ForceMode2D.Impulse);
    }

    private void UpdatePlayerState() {
        var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
        playerAnimator?.SetFloat("Velocity_Y", localVelocity.y);
        IsGrounded = Physics2D.BoxCast(
            playerCollider.bounds.center,
            // Creates a 0.1f thick box cast
            new Vector2(playerCollider.size.x, 0.1f),
            physicsManager.RelativeRotation,
            physicsManager.GravityDirection,
            // The box travels by distanceToGround units
            playerCollider.size.y / 2 + DistanceToGround,
            ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Door"))
        );
    }
}
