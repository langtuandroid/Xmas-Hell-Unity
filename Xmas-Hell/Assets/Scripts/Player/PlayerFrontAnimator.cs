using System.Collections;
using UnityEngine;

public class PlayerFrontAnimator : MonoBehaviour
{
    public Animator Animator;

	void OnEnable()
    {
        Animator.Play("Idle");
        StartCoroutine("StartRandomAnimationLoop");
	}

    void OnDisable()
    {
        StopCoroutine("StartRandomAnimationLoop");
        Animator.Play("Idle");
    }

    private IEnumerator StartRandomAnimationLoop()
    {
        Debug.Log("StartRandomAnimationLoop");

        Animator.Play("CoucouSide", 0, 0);

        Debug.Log("Idle animation? => " + Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        Debug.Log("Animation time finished? => " + (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .99f));

        while (Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .99f)
        {
            yield return null;
        }

        Animator.Play("Idle", 0, 0);
        yield return new WaitForSeconds(Random.Range(1, 5));
        StartCoroutine("StartRandomAnimationLoop");
    }
}
