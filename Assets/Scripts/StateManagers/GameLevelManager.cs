using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameLevelManager : Manager<GameLevelManager> {

    public GameObject Circle;
    public RectTransform FadeCircleTransform;
    public Animator FadeAnimator;

    private PlayerStateManager playerStateManager;
    private GameEventManager gameEventManager;

    public int MaxLevel { get; } = 6;

    public int CurrentLevel { get; private set; }

    protected override void Init() {
        CurrentLevel = 0;
    }

    void Start() {
        playerStateManager = PlayerStateManager.Instance;
        gameEventManager = GameEventManager.Instance;
    }

    void Update() {
        if (CrossPlatformInputManager.GetButtonUp("Reset") && CurrentLevel > 0 && CurrentLevel <= MaxLevel) {
            StartCoroutine(LoadLevelCoroutine());
        }
    }

    public void NextLevel() {
        if (CurrentLevel < 6) {
            CurrentLevel += 1;
            StartCoroutine(LoadLevelCoroutine());
        } else {
            WinBannerController.Instance.ShowBanner();
            playerStateManager.Living = false;
        }
    }

    private IEnumerator LoadLevelCoroutine() {
        StartFadeoutAnim();
        var timeStart = Time.time;
        var anim_time = FadeAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(anim_time/2);
        GamePhysicsManager.Instance.HardResetRotation();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync($"Level{CurrentLevel}");
        while (!asyncLoad.isDone) {
            yield return null;
        }
        // New level loaded
        GameOverBannerController.Instance.HideBanner();
        CameraAnimationController.Instance.Reset();
        var levelMeta = LoadLevelMeta();
        var player = GameObject.FindWithTag("Player");
        playerStateManager.OnLevelChange(levelMeta.MaxHealth, levelMeta.MaxSpin, player);
        gameEventManager.CurrentLevelWinnable = true;
        var timeWait = Mathf.Max(0, anim_time/2 - (Time.time - timeStart));
        yield return new WaitForSeconds(timeWait);
        StartFadeinAnim();
        anim_time = FadeAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(anim_time);
        GameSoundManager.Instance.PlayBackgroundMusic();
        Circle.SetActive(false);
    }

    private void StartFadeoutAnim() {
        var player = GameObject.FindWithTag("Player");
        var transitOrigin = (player != null) ? player.transform : transform;
        Circle.SetActive(true);
        FadeCircleTransform.position = new Vector3(
            transitOrigin.position.x, 
            transitOrigin.position.y, 
            FadeCircleTransform.position.z
        );
        FadeAnimator.SetTrigger("Fadeout");
    }

    private void StartFadeinAnim() {
        var player = GameObject.FindWithTag("Player");
        var transitDestination = (player != null) ? player.transform : transform;
        FadeCircleTransform.position = new Vector3(
            transitDestination.position.x, 
            transitDestination.position.y, 
            FadeCircleTransform.position.z
        );
        FadeAnimator.SetTrigger("Fadein");
    }

    private LevelMeta LoadLevelMeta() {
        var levelMetaObj = GameObject.Find("Meta");
        if (levelMetaObj == null) {
            Debug.LogWarning("LoadLevelMeta: No \"Meta\" gameObject found, skipping...");
        }
        return ComponentUtils.GetOrCreate<LevelMeta>(levelMetaObj);
    }

}
