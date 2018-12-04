using UnityEngine;

public class BossBallButtonManager : MonoBehaviour
{
    public BossTypeToGameObjectDictionary BossBallButtons;

    [SerializeField] private BossStore _bossStore;

    void Start ()
    {
        foreach (var pair in BossBallButtons)
        {
            var currentBossBall = pair.Value.GetComponent<BossBall>();
            currentBossBall.SetBossType(pair.Key);

            var bossData = SaveSystem.GetBossData(pair.Key);

            if (bossData != null)
            {
                var bossState = EBossBallState.Unknown;

                if (bossData.WinCounter > 0)
                {
                    bossState = EBossBallState.Beaten;
                }
                else
                {
                    // Bosses available by default
                    if (bossData.Type == EBoss.XmasBell ||
                        bossData.Type == EBoss.XmasCandy ||
                        bossData.Type == EBoss.XmasBall ||
                        bossData.Type == EBoss.XmasSnowflake)
                    {
                        bossState = EBossBallState.Available;
                    }

                    // Check relationship
                    if (_bossStore.BossRelationships.ContainsKey(bossData.Type))
                    {
                        var bossRelationShip = _bossStore.BossRelationships[bossData.Type];
                        var boss1Data = SaveSystem.GetBossData(bossRelationShip.Boss1);
                        var boss2Data = SaveSystem.GetBossData(bossRelationShip.Boss2);

                        if (boss1Data.WinCounter > 0 && boss2Data.WinCounter > 0)
                        {
                            bossState = EBossBallState.Available;
                        }
                        else
                        {
                            bossState = EBossBallState.Unknown;
                        }
                    }
                }

                currentBossBall.SetState(bossState);
            }
            else
            {
                currentBossBall.SetState(EBossBallState.Unknown);
            }
        }
	}
}
