using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine;

public class PlayGamesServices : MonoBehaviour
{
    public static PlayGamesServices Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);

            Initialize();
        }
    }

    private void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
        PlayGamesPlatform.Activate();
    }

    public void SignIn(Action<bool, string> callback = null)
    {
        Social.localUser.Authenticate((bool success, string error) =>
        {
            if (success)
                Debug.Log("User logged in with GPGS.");
            else
                Debug.LogError("Error during GPGS sign in: " + error);

            callback?.Invoke(success, error);
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
        Social.ReportProgress(id, 100f, success => 
        {
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

    public void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success =>
        {
            if (!success)
                Debug.LogError("Error adding a score to the leaderboard with ID: " + leaderboardId);
        });
    }

    #endregion
}
