using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovementController : MonoBehaviour {
    /// <summary>
    /// The follow speed of the key
    /// </summary>
    public float FollowSpeed = 1f;
    private Transform playerTransform = null;
    private SpriteRenderer playerSpriteRenderer = null;

    private Vector3 childPosition;

    void Start() {
        childPosition = transform.GetChild(0).localPosition;
    }

    void LateUpdate() {
        if (playerTransform != null) {
            transform.position = Vector2.Lerp(
                transform.position,
                playerTransform.position,
                Time.deltaTime * FollowSpeed
            );
        }
        if (playerSpriteRenderer != null) {
            var child = transform.GetChild(0);
            if (!playerSpriteRenderer.flipX) {
                child.localPosition = Vector3.Lerp(
                    child.localPosition, 
                    new Vector3(
                        -Mathf.Abs(childPosition.x),
                        childPosition.y,
                        childPosition.z
                    ),
                    Time.deltaTime * FollowSpeed
                );
            } else {
                child.localPosition = Vector3.Lerp(
                    child.localPosition, 
                    new Vector3(
                        Mathf.Abs(childPosition.x),
                        childPosition.y,
                        childPosition.z
                    ),
                    Time.deltaTime * FollowSpeed
                );
            }
        }
    }

    public void RegisterPlayer(GameObject player) {
        playerTransform = player.transform;
        playerSpriteRenderer = ComponentUtils.Get<SpriteRenderer>(player);
    }
}
