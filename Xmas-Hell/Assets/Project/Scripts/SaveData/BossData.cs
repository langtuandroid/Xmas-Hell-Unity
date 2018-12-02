using System;

[Serializable]
public class BossData
{
    public string Name = "";
    public bool Beaten = false;
    public float TimeSpent = 0; // Time spent on this boss in seconds
    public float BestScore = 0; // Best time to beat the boss in milliseconds
    public int DeathCounter = 0; // Count the times the player died against this boss

    public BossData(string name)
    {
        Name = name;
    }
}
