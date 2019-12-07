using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

public class GamePhysicsManager : Manager<GamePhysicsManager> {
    public float GravityScale = 3;
    public float RotationSpeed = 360f;

    public event EventHandler OnGravityChange;

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
    public Quaternion Rotation { get; private set; } = Quaternion.identity;

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
        Rotation = Quaternion.Euler(0, 0, RelativeRotation);
    }

    public void FixedUpdate() {

    }

    public void Update() {
        Rotation = Quaternion.RotateTowards(
            Rotation,
            Quaternion.Euler(0, 0, RelativeRotation),
            RotationSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Changes the direction of gravity by 90 degree
    /// </summary>
    /// <param name="direction">Positive value: clockwise, negative value: anti-clockwise</param>
    public void ChangeGravity(float direction) {
        OnGravityChange?.Invoke(this, EventArgs.Empty);
        RelativeRotation += direction * 90;
    }

    /// <summary>
    /// Reset rotation and gravity to default
    /// </summary>
    public void HardResetRotation() {
        RelativeRotation = 0;
        Rotation = Quaternion.Euler(0, 0, RelativeRotation);
    }
}
