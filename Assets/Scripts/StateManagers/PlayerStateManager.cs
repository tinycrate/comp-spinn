using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : Manager<PlayerStateManager> {

    public GameObject CurrentPlayerReference {
        get {
            if (currentPlayerReference != null) {
                return currentPlayerReference;
            }
            Debug.LogWarning("PlayerStateManager: Searching for player reference on the go... " +
                             "This should only happen when GameManager is manually placed on scene. " +
                             "Otherwise GameLevelManager is having problem setting player reference.");
            currentPlayerReference = GameObject.FindWithTag("Player");
            return currentPlayerReference;
        }
        set => currentPlayerReference = value;
    }

    public int Health { get; private set; } = -1;
    public int SpinLeft { get; private set; } = -1;

    public int MaxHealth { get; private set; } = -1;
    public int SpinLimit { get; private set; } = -1;

    public bool KeyObtained { get; set; } = false;

    public bool DoorReached { get; set; } = false;

    public bool Hurt { get; set; } = false;

    public bool Spinning { get; set; } = false;

    public bool Living { get; set; } = true;

    public float DoorStayedTime { get; private set; } = 0;

    private GameObject currentPlayerReference;

    void Update() {
        if (!DoorReached) {
            DoorStayedTime = 0;
        } else {
            DoorStayedTime += Time.deltaTime;
        }
    }

    public void OnLevelChange(int maxHealth, int spinLimit, GameObject player) {
        CurrentPlayerReference = player;
        MaxHealth = maxHealth;
        SpinLimit = spinLimit;
        Health = (MaxHealth >= 0) ? MaxHealth : -1;
        SpinLeft = (SpinLimit >= 0) ? SpinLimit : -1;
        KeyObtained = false;
        DoorReached = false;
        Spinning = false;
        Hurt = false;
        Living = true;
    }

    public void DeductHealth(int amount) {
        if (Health < 0) return;
        Health = Mathf.Max(0, Health - amount);
    }

    public void UseSpin() {
        if (SpinLeft <= 0) return;
        SpinLeft -= 1;
    }
}
