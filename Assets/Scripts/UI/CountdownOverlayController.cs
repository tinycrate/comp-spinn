using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownOverlayController : MonoBehaviour {

    /// <summary>
    /// Number of seconds before timer shows up
    /// </summary>
    public float DisplayAfterSeconds = 0.5f;
    /// <summary>
    /// Number of seconds to countdown, including DisplayAfterSeconds
    /// </summary>
    public float CountdownTime = 3f;

    private PlayerStateManager playerStateManager;
    private GameEventManager gameEventManager;
    private Image timerIcon;

    void Start() {
        playerStateManager = PlayerStateManager.Instance;
        gameEventManager = GameEventManager.Instance;
        timerIcon = ComponentUtils.Get<Image>(transform.GetChild(0).gameObject);
        timerIcon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update() {
        if (gameEventManager.CurrentLevelWinnable &&
            playerStateManager.DoorReached && 
            playerStateManager.KeyObtained && 
            playerStateManager.DoorStayedTime > DisplayAfterSeconds
        ) {
            transform.position = playerStateManager.CurrentPlayerReference.transform.position;
            timerIcon.fillAmount = Mathf.Min(
                1,
                (playerStateManager.DoorStayedTime - DisplayAfterSeconds) / (CountdownTime - DisplayAfterSeconds)
            );
        } else {
            timerIcon.fillAmount = 0;
        }
    }
}
