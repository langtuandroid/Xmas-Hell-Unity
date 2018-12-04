using UnityEngine;

public class BossBallButtonManager : MonoBehaviour
{
    public BossTypeToGameObjectDictionary BossBallButtons;

    void Start ()
    {
        foreach (var pair in BossBallButtons)
        {
            var currentBossBall = pair.Value.GetComponent<BossBall>();
            currentBossBall.SetBossType(pair.Key);

            var bossData = SaveSystem.GetBossData(pair.Key);

            if (bossData != null)
            {
                // Bosses available by default
                if (bossData.Type == EBoss.XmasBell ||
                    bossData.Type == EBoss.XmasCandy ||
                    bossData.Type == EBoss.XmasBall ||
                    bossData.Type == EBoss.XmasSnowflake)
                {
                    currentBossBall.SetState(EBossBallState.Available);
                }
            }
            else
            {
                currentBossBall.SetState(EBossBallState.Unknown);
            }
        }
	}
	
	void Update ()
    {
		
	}
}
