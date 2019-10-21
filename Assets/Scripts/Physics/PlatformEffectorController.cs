using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEffectorController : MonoBehaviour {
    private PlatformEffector2D platformEffector2D = null;
    private GamePhysicsManager physicsManager = null;

    void Start() {
        platformEffector2D = ComponentUtils.GetOrCreate<PlatformEffector2D>(gameObject);
        physicsManager = GamePhysicsManager.Instance;
    }

    void FixedUpdate() {
        if (!Mathf.Approximately(platformEffector2D.rotationalOffset, physicsManager.RelativeRotation)) {
            platformEffector2D.rotationalOffset = physicsManager.RelativeRotation;
        }
    }
}
