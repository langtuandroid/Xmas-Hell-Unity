using System.Collections.Generic;

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
}
