using JetBrains.Annotations;
using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    [UsedImplicitly]
    public void OnClick()
    {
        PlayGamesServices.Instance.ShowLeaderboardUI();
    }
}
