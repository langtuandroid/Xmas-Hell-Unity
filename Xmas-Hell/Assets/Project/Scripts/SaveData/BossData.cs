using System;

[Serializable]
public class BossData
{
    public EBoss Type = EBoss.Unknown;
    public float TotalTime = 0; // Time spent on this boss in seconds
    public float BestTime = 0; // Best time to beat the boss in milliseconds
    public int WinCounter = 0; // Count the times the player defeat this boss
    public int LoseCounter = 0; // Count the times the player died against this boss

    public BossData(EBoss bossType)
    {
        Type = bossType;
    }
}
