public class BossData
{
    string Name;
    bool Beaten;
    float TimeSpent; // Time spent on this boss in seconds
    float BestScore; // Best time to beat the boss in milliseconds
    int DeathCounter; // Count the times the player died against this boss

    public BossData(string name)
    {
        Name = name;
        Beaten = false;
        TimeSpent = 0;
        BestScore = 0;
        DeathCounter = 0;
    }
}
