using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuScreenManager : MonoBehaviour
{
    public Animator MenuAnimator;

    public void Start()
    {
        if (ScreenManager.GetPreviousScreen() == EScreen.Game)
            MenuAnimator.Play("GoToBossSelection", 0, 1f);

    }

    public void GoToBossSelection()
    {
        MenuAnimator.SetFloat("SpeedMultiplier", 1f);
        MenuAnimator.Play("GoToBossSelection");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuAnimator.SetFloat("SpeedMultiplier", -1f);
            MenuAnimator.Play("GoToBossSelection");
        }

        // Make sure we stop the animation when we played it entirely
        if ((MenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && MenuAnimator.GetFloat("SpeedMultiplier") > 0) ||
            (MenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && MenuAnimator.GetFloat("SpeedMultiplier") < 0))
        {
            MenuAnimator.SetFloat("SpeedMultiplier", 0f);
        }
    }
}
