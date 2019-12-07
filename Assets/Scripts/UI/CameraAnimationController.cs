using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationController : Singleton<CameraAnimationController> {

    private Animator animator;

    protected override void Awake() {
        base.Awake();
        animator = ComponentUtils.Get<Animator>(gameObject);
    }

    public IEnumerator PlayHurt() {
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public IEnumerator PlaySpin() {
        animator.SetTrigger("Spin");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
    }

    public IEnumerator PlaySpinFailed() {
        animator.SetTrigger("SpinFailed");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
    }
    public IEnumerator PlayDeath() {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void Reset() {
        animator.SetTrigger("Reset");
    }

}
