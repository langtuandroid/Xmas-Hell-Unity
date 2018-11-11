using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerFrontAnimator : MonoBehaviour
{
    public Animator Animator;
    public ScreenCornerToGameObjectDictionary AnchorHolders;
    public GameObject PlayerFront;
    public float StartAnimationDelay;
    public float NextAnimationRandomTimeMin;
    public float NextAnimationRandomTimeMax;

    private Vector3 _initialLocalPosition;
    private Vector3 _initialLocalScale;
    private float _nextAnimationRandomTimeMin;
    private bool _stopAnimation;

    private void Start()
    {
        _initialLocalPosition = PlayerFront.transform.localPosition;
        _initialLocalScale = PlayerFront.transform.localScale;
        _nextAnimationRandomTimeMin = StartAnimationDelay + NextAnimationRandomTimeMin;

        StartCoroutine(StartRandomAnimationLoop());
    }

    public void StopAnimation(bool value)
    {
        _stopAnimation = value;
    }

    private IEnumerator StartRandomAnimationLoop()
    {
        while (_stopAnimation)
            yield return null;

        yield return new WaitForSeconds(Random.Range(_nextAnimationRandomTimeMin, NextAnimationRandomTimeMax));

        if (Random.value > 0.5f)
            PlayCoucouUpAnimation();
        else
            PlayCoucouSideAnimation();

        var animatorIsIdle = Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        var animatorCurrentTime = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        // Make sure we wait until we switch from Idle to random animation
        yield return new WaitForEndOfFrame();

        animatorIsIdle = Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        animatorCurrentTime = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            yield return null;
        }

        _nextAnimationRandomTimeMin = NextAnimationRandomTimeMin;

        StartCoroutine(StartRandomAnimationLoop());
    }

    private void PlayCoucouSideAnimation()
    {
        var newLocalPosition = PlayerFront.transform.localPosition;
        newLocalPosition.x = 0;
        newLocalPosition.y = Random.Range(Screen.height * 0.4f, Screen.height * 1.1f);

        var newLocalScale = PlayerFront.transform.localScale;
        newLocalScale.x = _initialLocalScale.x;
        newLocalScale.y = _initialLocalScale.y;

        float randomSide = Random.value;

        if (randomSide > 0.5f)
        {
            PlayerFront.transform.SetParent(AnchorHolders[EScreenCorner.BottomRight].transform);
            newLocalScale.x *= -1f;
        }
        else
        {
            PlayerFront.transform.SetParent(AnchorHolders[EScreenCorner.BottomLeft].transform);
        }

        PlayerFront.transform.localScale = newLocalScale;
        PlayerFront.transform.localPosition = newLocalPosition;

        Animator.SetTrigger("PlayCoucouSide");
    }

    private void PlayCoucouUpAnimation()
    {
        var newLocalPosition = PlayerFront.transform.localPosition;
        newLocalPosition.x = Random.Range(Screen.width * 0.5f, Screen.width);
        newLocalPosition.y = 0;

        var newLocalScale = PlayerFront.transform.localScale;
        newLocalScale.x = _initialLocalScale.x;
        newLocalScale.y = _initialLocalScale.y;

        float randomSide = Random.value;

        if (Random.value > 0.5f)
        {
            PlayerFront.transform.SetParent(AnchorHolders[EScreenCorner.TopLeft].transform);
            newLocalScale.y *= -1f;
        }
        else
        {
            PlayerFront.transform.SetParent(AnchorHolders[EScreenCorner.BottomLeft].transform);
        }

        PlayerFront.transform.localScale = newLocalScale;
        PlayerFront.transform.localPosition = newLocalPosition;

        Animator.SetTrigger("PlayCoucouUp");
    }
}
