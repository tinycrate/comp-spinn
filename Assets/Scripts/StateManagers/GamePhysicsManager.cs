using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

public class GamePhysicsManager : Manager<GamePhysicsManager> {
    public float GravityScale = 3;
    public float RotationSpeed = 360f;

    /// <summary>
    /// Sets or gets the gravity direction in Vector2.
    /// Only directional information is preserved.
    /// </summary>
    public Vector2 GravityDirection {
        get => gravityDirection;
        set {
            value.x = Mathf.Abs(value.x) < 0.0001f ? 0 : Mathf.Sign(value.x);
            value.y = Mathf.Abs(value.y) < 0.0001f ? 0 : Mathf.Sign(value.y);
            gravityDirection = value;
        }
    }
    private Vector2 gravityDirection = Vector2.down;

    /// <summary>
    /// Gets the current gravity scaled from Physics2D.gravity.magnitude
    /// </summary>
    public Vector2 Gravity => GravityDirection * GravityScale * Physics2D.gravity.magnitude;

    /// <summary>
    /// Provides a rotation value for objects so they can be rotated according to
    /// gravitational changes. Smoothing is applied.
    /// </summary>
    public Quaternion Rotation => rotation;

    // This backing field is for Unity Inspector
    private Quaternion rotation = Quaternion.identity;

    /// <summary>
    /// Rotation in degree relative to the world space downward direction
    /// Setting the rotation will update GravityDirection to match up accordingly
    /// </summary>
    public float RelativeRotation {
        get => Vector2.SignedAngle(Vector2.down, GravityDirection);
        set {
            var rad = (value - 90) * Mathf.Deg2Rad;
            GravityDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }

    protected override void Init() {
        GravityDirection = Vector2.down;
        rotation = Quaternion.Euler(0, 0, RelativeRotation);
    }

    public void FixedUpdate() {

    }

    public void Update() {
        rotation = Quaternion.RotateTowards(
            Rotation,
            Quaternion.Euler(0, 0, RelativeRotation),
            RotationSpeed * Time.deltaTime
        );
        if (CrossPlatformInputManager.GetButtonDown("AltHorizontal")) {
            var direction = Mathf.Sign(CrossPlatformInputManager.GetAxisRaw("AltHorizontal"));
            RelativeRotation += direction * 90;
        }
    }
}
