using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatOverlayController : MonoBehaviour {

    public GameObject HeartsContainerObject;
    public GameObject SpinsContainerObject;

    /// <summary>
    /// Sets a time delay between each element update for aesthetic purposes
    /// </summary>
    public float UpdateDelay = 0.1f;
    
    /// <summary>
    /// Spacing between elements (Percentage value of the elements' width)
    /// </summary>
    public float Spacing = 0.1f;

    public GameObject HeartPrefab;
    public GameObject SpinPrefab;

    public int CurrentHeartCount => Hearts.Count;
    public int CurrentSpinCount => Spins.Count;

    private PlayerStateManager playerStateManager;

    private Stack<RectTransform> Hearts = new Stack<RectTransform>();
    private Stack<RectTransform> Spins = new Stack<RectTransform>();

    private bool overlayUpdating = false;

    void Start() {
        playerStateManager = PlayerStateManager.Instance;
        if (HeartsContainerObject == null) {
            Debug.Log("Container for heats not set! Position might be off.");
            HeartsContainerObject = gameObject;
        }
        if (SpinsContainerObject == null) {
            Debug.Log("Container for spins not set! Position might be off.");
            SpinsContainerObject = gameObject;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!overlayUpdating) {
            overlayUpdating = true;
            StartCoroutine(UpdateOverlay());
        }
    }

    private IEnumerator UpdateOverlay() {
        yield return StartCoroutine(UpdateHealthOverlay());
        yield return StartCoroutine(UpdateSpinOverlay());
        overlayUpdating = false;
    }

    private IEnumerator UpdateHealthOverlay() {
        while (CurrentHeartCount < playerStateManager.Health) {
            var heart = ComponentUtils.Get<RectTransform>(
                Instantiate(HeartPrefab, HeartsContainerObject.transform).transform.GetChild(0).gameObject
            );
            var width = heart.sizeDelta.x;
            heart.anchoredPosition = new Vector2(
                heart.anchoredPosition.x - (width * 1.1f) * CurrentHeartCount,
                heart.anchoredPosition.y
            );
            Hearts.Push(heart);
            yield return new WaitForSeconds(UpdateDelay);
        }
        while (CurrentHeartCount > 0 && CurrentHeartCount > playerStateManager.Health) {
            var heart = Hearts.Pop();
            StartCoroutine(DestroyElementAnimated(heart));
            yield return new WaitForSeconds(UpdateDelay);
        }
    }

    private IEnumerator DestroyElementAnimated(RectTransform element) {
        var animator = ComponentUtils.Get<Animator>(element.gameObject);
        animator.SetTrigger("Drop");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(element.parent.gameObject);
    }

    private IEnumerator UpdateSpinOverlay() {
        while (CurrentSpinCount < playerStateManager.SpinLeft) {
            var spin = ComponentUtils.Get<RectTransform>(
                Instantiate(SpinPrefab, SpinsContainerObject.transform).transform.GetChild(0).gameObject
            );
            var width = spin.sizeDelta.x;
            spin.anchoredPosition = new Vector2(
                spin.anchoredPosition.x - (width * 1.1f) * CurrentSpinCount,
                spin.anchoredPosition.y
            );
            Spins.Push(spin);
            yield return new WaitForSeconds(UpdateDelay);
        }
        while (CurrentSpinCount > 0 && CurrentSpinCount > playerStateManager.SpinLeft) {
            var spin = Spins.Pop();
            Destroy(spin.parent.gameObject);
            yield return new WaitForSeconds(UpdateDelay);
        }
    }

}
