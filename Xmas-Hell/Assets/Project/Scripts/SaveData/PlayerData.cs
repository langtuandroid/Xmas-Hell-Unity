using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{
    public int DeathCounter;
    public double PlayTime;
    public List<BossData> BossesData;

    public PlayerData()
    {
        // Initialize default player data
        DeathCounter = 0;
        PlayTime = 0;
        BossesData = new List<BossData>()
        {
            new BossData("XmasBall"),
            new BossData("XmasBell"),
            new BossData("XmasSnowflake"),
            new BossData("XmasCandy"),
            new BossData("XmasGift"),
            new BossData("XmasLog"),
            new BossData("XmasTree"),
            new BossData("XmasReindeer"),
            new BossData("XmasSnowman"),
            new BossData("XmasSanta")
        };
    }

    public void BossWon(string bossName, float time)
    {
        var boss = BossesData.FirstOrDefault(b => b.Name == bossName);

        if (boss != null)
        {
            boss.WinCounter++;
            boss.TotalTime += time;

            if (boss.BestTime > time)
            {
                boss.BestTime = time;
            }

            PlayGamesServices.Instance.SaveData();
        }
        else
        {
            Debug.LogError("No boss found with this name in the player data: " + bossName);
        }
    }

    public void BossLost(string bossName, float time)
    {
        var boss = BossesData.FirstOrDefault(b => b.Name == bossName);

        if (boss != null)
        {
            boss.LoseCounter++;
            boss.TotalTime += time;

            PlayGamesServices.Instance.SaveData();
        }
        else
        {
            Debug.LogError("No boss found with this name in the player data: " + bossName);
        }
    }
}
