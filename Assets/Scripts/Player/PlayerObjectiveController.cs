using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectiveController : MonoBehaviour {
    public GameObject KeyItemPrefab;
    
    private PlayerStateManager playerStateManager;

    void Start() {
        playerStateManager = PlayerStateManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Key") {
            playerStateManager.KeyObtained = true;
            var keyItem = Instantiate(KeyItemPrefab, Vector3.zero, Quaternion.identity);
            keyItem.transform.position = col.ClosestPoint(transform.position);
            ComponentUtils.Get<KeyMovementController>(keyItem)?.RegisterPlayer(gameObject);
            Destroy(col.gameObject);
        }
        if (col.gameObject.name == "Door") {
            playerStateManager.DoorReached = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Door") {
            playerStateManager.DoorReached = false;
        }
    }

}
