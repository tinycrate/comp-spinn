using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControlledGravity : MonoBehaviour {
    private ConstantForce2D gravityForce = null;
    private GamePhysicsManager physicsManager = null;

    void Awake() {
        gravityForce = gameObject.AddComponent<ConstantForce2D>();
        var rigidBody2D = ComponentUtils.GetOrCreate<Rigidbody2D>(gameObject);
        rigidBody2D.gravityScale = 0;
    }

    void Start() {
        physicsManager = GamePhysicsManager.Instance;
        gravityForce.force = physicsManager.Gravity;
    }

    // Update is called once per frame
    void Update() {
        gravityForce.force = physicsManager.Gravity;
    }
}
