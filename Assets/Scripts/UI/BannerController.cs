using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BannerController<T> : Singleton<T> where T : BannerController<T> {
    private GameObject child;
    protected override void Awake() {
        base.Awake();
        child = transform.GetChild(0).gameObject;
    }
    public void ShowBanner() {
        child.SetActive(true);
    }

    public void HideBanner() {
        child.SetActive(false);
    }
}