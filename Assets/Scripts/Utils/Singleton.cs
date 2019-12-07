using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    /// <summary>
    /// Gets an instance of the singleton
    /// </summary>
    public static T Instance {
        get {
            if (instance != null) return instance;
            Debug.LogError($"Failed to get an instance of {typeof(T).Name}." +
                           $"It is either not initialized yet (Accessed on Awake()) " +
                           $"or it is not even in the scene!");
            return null;
        }
    }
    private static T instance = null;

    protected virtual void Awake() {
        if (instance == null) {
            instance = (T) this;
        }
        else if (instance != this) {
            Debug.LogWarning(
                "There could be only one instance of " +
                $"{GetType().Name} in the game. The script attached to " +
                $"the current game object is destroyed. (In {gameObject.name})",
                gameObject
            );
            
            Destroy(this);
            return;
        }
    }
}