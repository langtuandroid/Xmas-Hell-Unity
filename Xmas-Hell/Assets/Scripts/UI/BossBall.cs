using UnityEngine;

public class BossBall : MonoBehaviour
{
    public EBoss BossType;
    public Animator Animator;

    public void Start()
    {
        Animator.speed = Random.Range(0.5f, 1.5f);
        Animator.Play("Jiggle", 0, Random.Range(0f, 1f));
    }

    public void OnClick()
    {
        SessionData.SelectedBoss = BossType;
        ScreenManager.GoToScreen(EScreen.Game);
    }
}
