using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    private const string SAVE_NAME = "XmasSave";
    private static PlayerData _playerData;
    private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Load();
    }

    public static void Save()
    {
        InitializeJsonSerializerSettings();

        string stringPlayerData = JsonConvert.SerializeObject(_playerData, _jsonSerializerSettings);

        // Local save
        PlayerPrefs.SetString(SAVE_NAME, stringPlayerData);
    }

    public static void Load()
    {
        InitializeJsonSerializerSettings();

        string stringPlayerData = PlayerPrefs.GetString(SAVE_NAME);

        PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(stringPlayerData, _jsonSerializerSettings);

        // First time
        if (playerData == null)
            playerData = new PlayerData();

        _playerData = playerData;

        Save();
    }

    #region Boss related methods

    public static BossData GetBossData(EBoss bossType)
    {
        var boss = _playerData.BossesData.FirstOrDefault(b => b.Type == bossType);

        if (boss == null)
        {
            Debug.LogError("No boss found in the player data with name: " + boss.Type);
        }

        return boss;
    }

    public static void BossEnd(EBoss bossType, bool win, float time)
    {
        var boss = GetBossData(bossType);

        if (boss != null)
        {
            boss.TotalTime += Mathf.Ceil(time); // seconds

            if (win)
            {
                boss.WinCounter++;

                if (boss.BestTime == 0 || boss.BestTime > time)
                    boss.BestTime = time * 1000f; // milliseconds
            }
            else
                boss.LoseCounter++;

            Save();
        }
        else
        {
            Debug.LogError("No boss found with this name in the player data: " + bossType);
        }
    }

    #endregion

    #region Utils

    private static void InitializeJsonSerializerSettings()
    {
        _jsonSerializerSettings.Formatting = Formatting.Indented;
        _jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        _jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
    }

    #endregion
}
