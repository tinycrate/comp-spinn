using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMeta : MonoBehaviour {
    public int MaxHealth = 0;
    public int MaxSpin = 0;
    void Awake() {
        if (gameObject.name != "Meta") {
            Debug.LogWarning("LevelMeta must be attached to a gameObject called \"Meta\" in order to be recognized");
        }
    }
}
