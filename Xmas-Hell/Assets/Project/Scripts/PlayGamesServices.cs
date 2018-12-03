using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;

public class PlayGamesServices : MonoBehaviour
{
    public static PlayGamesServices Instance { get; private set; }
    public CloudSave CloudSave;

    private const string SAVE_NAME = "Save";

    private bool _isSaving = false;
    private bool _isCloudDataLoaded = false;
    
    // Serialization/Deserialization
    private JsonSerializerSettings _jsonSerializerSettings;

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
        CloudSave = new CloudSave();

        _jsonSerializerSettings = new JsonSerializerSettings();
        _jsonSerializerSettings.Formatting = Formatting.Indented;
        _jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        _jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;

        if (!PlayerPrefs.HasKey(SAVE_NAME))
        {
            var stringPlayerData = JsonConvert.SerializeObject(CloudSave.PlayerData, _jsonSerializerSettings);
            PlayerPrefs.SetString(SAVE_NAME, stringPlayerData);
        }

        if (!PlayerPrefs.HasKey("IsFirstTime"))
            PlayerPrefs.SetInt("IsFirstTime", 1);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
        PlayGamesPlatform.Activate();

        // Load local save data
        LoadLocal();
    }

    #region Authentication

    public void SignIn(Action<bool, string> callback = null)
    {
        Social.localUser.Authenticate((bool success, string error) =>
        {
            if (success)
                Debug.Log("User logged in with GPGS.");
            else
                Debug.LogError("Error during GPGS sign in: " + error);

            callback?.Invoke(success, error);

            // Load data from cloud
            LoadData();
        });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    #endregion

    #region Save

    // -1 cloud save is more recent | 0 saves are equal | 1 local save is more recent
    private int CompareSave(PlayerData localSave, PlayerData cloudSave)
    {
        // TODO: Implement real comparison (field by field)
        return 0;
    }

    public string GameDataToString()
    {
        return JsonConvert.SerializeObject(CloudSave.PlayerData, _jsonSerializerSettings);
    }

    // This overload is used when user is connected to the interner
    // parsing string to game data, also deciding if we should use local or cloud save
    public void StringToGameData(string cloudData, string localData)
    {
        if (cloudData == string.Empty)
        {
            StringToGameData(localData);
            _isCloudDataLoaded = true;
            return;
        }

        if (localData == string.Empty)
        {
            CloudSave.PlayerData = JsonConvert.DeserializeObject<PlayerData>(cloudData, _jsonSerializerSettings);
            PlayerPrefs.SetString(SAVE_NAME, cloudData);
            _isCloudDataLoaded = true;
            return;
        }

        PlayerData localSave = JsonConvert.DeserializeObject<PlayerData>(localData, _jsonSerializerSettings);
        PlayerData cloudSave = JsonConvert.DeserializeObject<PlayerData>(cloudData, _jsonSerializerSettings);

        // If it's the first time that game has been launched after installing it and successfuly logging into GPGS
        if (PlayerPrefs.GetInt("IsFirstTime") == 1)
        {
            // 0 => false
            PlayerPrefs.SetInt("IsFirstTime", 0);

            // If it's the first time we load cloud data
            if (CompareSave(localSave, cloudSave) >= 0)
            {
                PlayerPrefs.SetString(SAVE_NAME, cloudData);
            }
        }
        // If it's not the first time, start comparing
        else
        {
            if (CompareSave(localSave, cloudSave) <= 0)
            {
                PlayerPrefs.SetString(SAVE_NAME, cloudData);
            }

            if (int.Parse(localData) > int.Parse(cloudData))
            {
                CloudSave.PlayerData = cloudSave;
                _isCloudDataLoaded = true;
                SaveData();

                // Update achievement/leaderboard
            }
        }
    }

    // This overload is used when there's no internet connection - loading only local data
    public void StringToGameData(string localData)
    {
        if (localData != string.Empty)
        {
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(localData, _jsonSerializerSettings);
            CloudSave.PlayerData = playerData;
        }
    }

    public void LoadData()
    {
        if (Social.localUser.authenticated)
        {
            _isSaving = false;

            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(
                SAVE_NAME, 
                DataSource.ReadCacheOrNetwork, 
                true, 
                ResolveConflict, 
                OnSavedGameOpened
            );
        }
        else
        {
            LoadLocal();
        }
    }

    private void LoadLocal()
    {
        SaveSystem.Load();
    }

    public void SaveData()
    {
        if (!_isCloudDataLoaded)
        {
            SaveLocal();
            return;
        }

        if (Social.localUser.authenticated)
        {
            _isSaving = true;

            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(
                SAVE_NAME,
                DataSource.ReadCacheOrNetwork,
                true,
                ResolveConflict,
                OnSavedGameOpened
            );
        }
        else
        {
            SaveLocal();
        }
    }

    private void SaveLocal()
    {
        SaveSystem.Save();
    }

    private void ResolveConflict(
        IConflictResolver resolver, 
        ISavedGameMetadata original, 
        byte[] originalData, 
        ISavedGameMetadata unmerged, 
        byte[] unmergedData)
    {
        if (originalData == null)
            resolver.ChooseMetadata(unmerged);
        else if (unmergedData == null)
            resolver.ChooseMetadata(original);
        else
        {
            string originalString = Encoding.ASCII.GetString(originalData);
            string unmergedString = Encoding.ASCII.GetString(unmergedData);

            // TODO: Compare the save
            resolver.ChooseMetadata(unmerged);
        }
    }

    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (!_isSaving)
                LoadGame(game);
            else
                SaveGame(game);
        }
        else
        {
            if (!_isSaving)
                LoadLocal();
            else
                SaveLocal();
        }
    }

    private void LoadGame(ISavedGameMetadata game)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
    }

    private void SaveGame(ISavedGameMetadata game)
    {
        string stringToSave = GameDataToString();
        SaveLocal();

        byte[] dataToSave = Encoding.ASCII.GetBytes(stringToSave);

        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();

        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, dataToSave, OnSavedGameDataWritten);
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] savedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string cloudDataString = Encoding.ASCII.GetString(savedData);

            if (savedData.Length == 0)
                cloudDataString = string.Empty;
            else
                cloudDataString = Encoding.ASCII.GetString(savedData);

            string localDataString = PlayerPrefs.GetString(SAVE_NAME);

            StringToGameData(cloudDataString, localDataString);
        }
    }

    private void OnSavedGameDataWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log("Saved data to the cloud!");
    }

    #endregion

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
