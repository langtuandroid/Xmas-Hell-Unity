using JetBrains.Annotations;
using UnityEngine;

public class AchievementButton : MonoBehaviour
{
    [UsedImplicitly]
    public void OnClick()
    {
        PlayGamesServices.Instance.ShowAchievementsUI();
    }
}
