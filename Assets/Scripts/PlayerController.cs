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
    private Collider2D playerCollider;
    private GamePhysicsManager physicsManager;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

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
    public bool IsGrounded { get; private set; }

    void Awake() {
        rigidBody2D = ComponentUtils.GetOrCreate<Rigidbody2D>(gameObject);
        playerCollider = ComponentUtils.Get<Collider2D>(gameObject);
        playerAnimator = ComponentUtils.GetOrNull<Animator>(gameObject);
        spriteRenderer = ComponentUtils.Get<SpriteRenderer>(gameObject);
    }

    void Start() {
        physicsManager = GamePhysicsManager.Instance;
        physicsManager.OnGravityChange += OnGravityChange;
    }

    void FixedUpdate() {
        HandleScheduledJump();
    }

    // Update is called once per frame
    void Update() {
        UpdatePlayerState();
        HandleInput();
    }

    private void HandleInput() {
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        if (CrossPlatformInputManager.GetButton("Horizontal")) {
            playerAnimator?.SetBool("Run", true);
            spriteRenderer.flipX = inputX < 0;
            var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
            rigidBody2D.velocity = transform.TransformDirection(new Vector2(inputX * Speed, localVelocity.y));
        } else {
            playerAnimator?.SetBool("Run", false);
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
        playerAnimator?.SetFloat("Velocity_Y", rigidBody2D.velocity.y);
        IsGrounded = Physics2D.BoxCast(
            playerCollider.bounds.center,
            // Creates a 0.1f thick box cast
            new Vector2(playerCollider.bounds.size.x, 0.1f),
            physicsManager.RelativeRotation,
            physicsManager.GravityDirection,
            // The box travels by distanceToGround units
            playerCollider.bounds.extents.y + DistanceToGround,
            ~LayerMask.GetMask("Player")
        );
        playerAnimator?.SetBool("Air", !IsGrounded);
    }
}
