using UnityEngine;

public class BossBall : MonoBehaviour
{
    public EBoss BossType;

    public void OnClick()
    {
        SessionData.SelectedBoss = BossType;
        ScreenManager.GoToScreen(EScreen.Game);
    }
}
