using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardGravityRotator : MonoBehaviour  {
    private GamePhysicsManager physicsManager = null;
    // Start is called before the first frame update
    void Start() {
        physicsManager = GamePhysicsManager.Instance;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = physicsManager.Rotation;
    }
}
