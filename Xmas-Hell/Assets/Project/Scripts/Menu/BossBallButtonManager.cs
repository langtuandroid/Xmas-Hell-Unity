using UnityEngine;

public class BossBallButtonManager : MonoBehaviour
{
    public BossTypeToGameObjectDictionary BossBallButtons;

    void Start ()
    {
        foreach (var pair in BossBallButtons)
        {
            var currentBossBall = pair.Value.GetComponent<BossBall>();

            currentBossBall.SetState(EBossBallState.Available);
            currentBossBall.SetBossType(pair.Key);
        }
	}
	
	void Update ()
    {
		
	}
}
