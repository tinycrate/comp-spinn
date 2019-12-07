using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : Singleton<T> where T : Manager<T> {
    /// <summary>
    /// Initialize the script on Unity Awake
    /// </summary>
    protected virtual void Init() { }

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Init();
    }
}
