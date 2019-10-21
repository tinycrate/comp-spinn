using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float Speed = 10;
    public float JumpForce = 10;
    private Rigidbody2D rigidBody2D = null;
    private float distanceToGround = 0;

    void Awake() {
        rigidBody2D = ComponentUtils.GetOrCreate<Rigidbody2D>(gameObject);
        var playerCollider = ComponentUtils.Get<Collider2D>(gameObject);
        distanceToGround = playerCollider.bounds.extents.y + 0.1f;
    }

    void FixedUpdate() {
    }

    // Update is called once per frame
    void Update() {
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        if (CrossPlatformInputManager.GetButton("Horizontal")) {
            var localVelocity = transform.InverseTransformDirection(rigidBody2D.velocity);
            rigidBody2D.velocity = transform.TransformDirection(new Vector2(inputX * Speed, localVelocity.y));
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump")) {
            Debug.DrawRay(transform.position, new Vector3(0, -distanceToGround, 0), Color.red);
            rigidBody2D.AddRelativeForce(new Vector2(0,JumpForce), ForceMode2D.Impulse);
        }
    }
}
