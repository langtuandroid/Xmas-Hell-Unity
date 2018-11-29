﻿using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesServices : MonoBehaviour
{
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
        PlayGamesPlatform.Activate();

        SignIn();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log("User logged in with GPGS.");
        });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    #region Achievements

    public void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    public void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100f, success => {
            if (!success)
                Debug.LogError("Error unlocking the achievement with ID: " + id);
        });
    }

    public void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success =>
        {
            if (!success)
                Debug.LogError("Error incrementing the achievement with ID: " + id);
        });
    }

    #endregion

    #region Leaderboard

    public void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }

    #endregion
}
