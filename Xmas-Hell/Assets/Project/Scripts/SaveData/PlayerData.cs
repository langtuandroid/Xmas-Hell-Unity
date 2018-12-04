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
            new BossData(EBoss.XmasBall),
            new BossData(EBoss.XmasBell),
            new BossData(EBoss.XmasSnowflake),
            new BossData(EBoss.XmasCandy),
            new BossData(EBoss.XmasGift),
            new BossData(EBoss.XmasLog),
            new BossData(EBoss.XmasTree),
            new BossData(EBoss.XmasReindeer),
            new BossData(EBoss.XmasSnowman),
            new BossData(EBoss.XmasSanta)
        };
    }
}
