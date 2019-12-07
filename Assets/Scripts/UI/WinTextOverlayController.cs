using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTextOverlayController : Singleton<WinTextOverlayController> {

    private GameObject childObject;
    private Animator winTextAnimator;

    // Start is called before the first frame update
    void Start() {
        childObject = transform.GetChild(0).gameObject;
        winTextAnimator = ComponentUtils.Get<Animator>(childObject);
        childObject.SetActive(false);
    }

    public IEnumerator PlayWinAnimation() {
        childObject.SetActive(true);
        winTextAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(2.5f);
        childObject.SetActive(false);
    }
}
