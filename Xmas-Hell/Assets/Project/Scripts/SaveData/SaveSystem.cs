using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    public static PlayerData PlayerData;

    private const string SAVE_NAME = "XmasSave";
    private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Load();
    }

    public static void Save()
    {
        InitializeJsonSerializerSettings();

        string stringPlayerData = JsonConvert.SerializeObject(PlayerData, _jsonSerializerSettings);

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
        {
            playerData = new PlayerData();
        }

        PlayerData = playerData;
    }

    #region Boss related methods

    public static BossData GetBossData(EBoss bossType)
    {
        var boss = PlayerData.BossesData.FirstOrDefault(b => b.Type == bossType);

        if (boss == null)
        {
            Debug.LogError("No boss found in the player data with name: " + boss.Type);
        }

        return boss;
    }

    public static void BossWon(EBoss bossType, float time)
    {
        var boss = GetBossData(bossType);

        if (boss != null)
        {
            boss.WinCounter++;
            boss.TotalTime += time;

            if (boss.BestTime > time)
            {
                boss.BestTime = time;
            }

            Save();
        }
        else
        {
            Debug.LogError("No boss found with this name in the player data: " + bossType);
        }
    }

    public static void BossLost(EBoss bossType, float time)
    {
        var boss = GetBossData(bossType);

        if (boss != null)
        {
            boss.LoseCounter++;
            boss.TotalTime += time;

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
