using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerEventController : MonoBehaviour {

    private Animator animator;
    private PlayerStateManager playerStateManager;
    
    void Start() {
        playerStateManager = PlayerStateManager.Instance;
        animator = ComponentUtils.Get<Animator>(gameObject);
    }
    
    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.name == "Spike" && !playerStateManager.Hurt) {
            playerStateManager.Hurt = true;
            StartCoroutine(OnPlayerHurt());
        }
    }

    public void Update() {
        if (CrossPlatformInputManager.GetButtonDown("AltHorizontal") && !playerStateManager.Spinning && playerStateManager.Living) {
            playerStateManager.Spinning = true;
            StartCoroutine(OnPlayerSpin());
        }
    }

    private IEnumerator OnPlayerSpin() {
        if (playerStateManager.SpinLeft > 0) {
            playerStateManager.UseSpin();
        } else if (playerStateManager.SpinLeft == 0) {
            yield return StartCoroutine(CameraAnimationController.Instance.PlaySpinFailed());
            playerStateManager.Spinning = false;
            yield break;
        }
        var direction = Mathf.Sign(CrossPlatformInputManager.GetAxisRaw("AltHorizontal"));
        GamePhysicsManager.Instance.ChangeGravity(direction);
        yield return StartCoroutine(CameraAnimationController.Instance.PlaySpin());
        playerStateManager.Spinning = false;
    }

    private IEnumerator OnPlayerHurt() {
        animator.SetTrigger("Hurt");
        playerStateManager.DeductHealth(1);
        if (playerStateManager.Health == 0) {
            playerStateManager.Living = false;
        }
        var timeStart = Time.time;
        yield return StartCoroutine(CameraAnimationController.Instance.PlayHurt());
        var timeWait = Mathf.Max(
            0,
            animator.GetCurrentAnimatorStateInfo(1).length - (Time.time - timeStart)
        );
        if (!playerStateManager.Living) {
            ComponentUtils.Get<SpriteRenderer>(playerStateManager.CurrentPlayerReference).enabled = false;
            StartCoroutine(CameraAnimationController.Instance.PlayDeath());
            GameOverBannerController.Instance.ShowBanner();
            GameSoundManager.Instance.StopBackgroundMusic();
            GameSoundManager.Instance.PlaySound(GameSoundManager.GameOverAudioClip);
            playerStateManager.CurrentPlayerReference.SetActive(false);
            yield break;
        }
        yield return new WaitForSeconds(timeWait);
        playerStateManager.Hurt = false;
    }
}
