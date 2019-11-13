using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float Speed = 10;
    public float JumpLimit = 2;
    public float JumpForce = 10;
    public float DistanceToGround = 0.3f;
    public float JumpScheduleTime = 0.1f;

    private Rigidbody2D rigidBody2D = null;
    private Collider2D playerCollider;
    private GamePhysicsManager physicsManager;

    /// <summary>
    /// Whether a jump is being scheduled in the next JumpScheduleTime seconds
    /// When set to true, schedules a jump to be executed if the player lands in JumpScheduleTime time 
    /// </summary>
    public bool JumpScheduled {
        get => lastScheduledJump >= 0 && Time.time - lastScheduledJump < JumpScheduleTime;
        private set => lastScheduledJump = value ? Time.time : -1;
    }
    private float lastScheduledJump = -1;
    private float jumpsRemaining = 0;

    void Awake() {
        rigidBody2D = ComponentUtils.GetOrCreate<Rigidbody2D>(gameObject);
        playerCollider = ComponentUtils.Get<Collider2D>(gameObject);
    }

    void Start() {
        physicsManager = GamePhysicsManager.Instance;
    }

    void FixedUpdate() {
        if (JumpScheduled && jumpsRemaining > 0) {
            Jump();
            jumpsRemaining--;
        }
    }

    // Update is called once per frame
    void Update() {
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        if (CrossPlatformInputManager.GetButton("Horizontal")) {
            var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
            rigidBody2D.velocity = transform.TransformDirection(new Vector2(inputX * Speed, localVelocity.y));
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            JumpScheduled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (IsGrounded()) jumpsRemaining = JumpLimit;
    }

    private void Jump() {
        var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
        rigidBody2D.velocity = transform.TransformDirection(new Vector2(localVelocity.x, 0));
        rigidBody2D.AddRelativeForce(new Vector2(0,JumpForce), ForceMode2D.Impulse);
        JumpScheduled = false;
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(
            transform.position,
            // Creates a 0.1f thick box cast
            new Vector2(playerCollider.bounds.size.x, 0.1f),
            physicsManager.RelativeRotation,
            physicsManager.GravityDirection,
            // The box travels by distanceToGround units
            playerCollider.bounds.extents.y + DistanceToGround,
            ~LayerMask.GetMask("Player")
        );
    }
}
