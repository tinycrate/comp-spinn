using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Manager<GameEventManager> {

    public bool CurrentLevelWinnable { get; set; } = false;

    private PlayerStateManager playerStateManager;

    void Start() {
        playerStateManager = PlayerStateManager.Instance;
    }

    void Update() {
        if (CurrentLevelWinnable && playerStateManager.KeyObtained && playerStateManager.DoorReached &&
            playerStateManager.DoorStayedTime > 3) {
            CurrentLevelWinnable = false;
            StartCoroutine(WinLevel());
        }
    }

    private IEnumerator WinLevel() {
        playerStateManager.Living = false;
        var winTextOverlayController = WinTextOverlayController.Instance;
        yield return winTextOverlayController.PlayWinAnimation();
        GameLevelManager.Instance.NextLevel();
    }
}
