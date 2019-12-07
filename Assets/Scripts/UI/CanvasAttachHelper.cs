using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches Canvas defined in levels to the camera currently on scene
/// </summary>
public class CanvasAttachHelper : MonoBehaviour {

    private Canvas canvas;
    
    // Start is called before the first frame update
    void Start() {
        canvas = ComponentUtils.Get<Canvas>(gameObject);
        canvas.worldCamera = ComponentUtils.Get<Camera>(GameObject.Find("Main Camera"));
    }
}
