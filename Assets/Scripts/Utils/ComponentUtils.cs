using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nice wrapper for component related stuff with slightly more detailed debug info
/// </summary>
public static class ComponentUtils {
    /// <summary>
    /// Gets a component, create one if not found.
    /// </summary>
    /// <param name="origin">GameObject that the component is attached</param>
    /// <param name="showWarnings">Show debug warnings when component is not found</param>
    public static T GetOrCreate<T>(GameObject origin, bool showWarnings = true) where T : Component {
        T component = origin.GetComponent<T>();
        if (component != null) return component;
        if (showWarnings) {
            Debug.LogWarning($"Game object {origin.name} has no {typeof(T).Name} " +
                             "attached which it should. One is being generated on the fly.");
        }
        component = origin.AddComponent<T>();
        return component;
    }
    /// <summary>
    /// Gets a component, logs an error if not found.
    /// </summary>
    /// <param name="origin">GameObject that the component is attached</param>
    public static T Get<T>(GameObject origin) where T : Component {
        T component = origin.GetComponent<T>();
        if (component != null) return component;
        Debug.LogError($"Game object {origin.name} has no {typeof(T).Name} attached.");
        return null;
    }
}
