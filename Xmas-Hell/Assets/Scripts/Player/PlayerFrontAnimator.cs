using System.Collections;
using UnityEngine;

public class PlayerFrontAnimator : MonoBehaviour
{
    public Animator Animator;

	void Start ()
    {
        StartCoroutine("StartRandomAnimationLoop");
	}

    private IEnumerator StartRandomAnimationLoop()
    {
        Animator.Play("CoucouSide", 0, 0);

        while (Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .99f)
        {
            yield return null;
        }

        Animator.Play("Idle");
        yield return new WaitForSeconds(Random.Range(1, 5));
        StartCoroutine("StartRandomAnimationLoop");
    }
}
