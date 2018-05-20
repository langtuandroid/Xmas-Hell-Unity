using System;
using UnityEngine;

public enum EBoss
{
    Unknown,
    Debug,
    XmasBall,
    XmasBell,
    XmasCandy,
    XmasSnowflake,
    XmasLog,
    XmasTree,
    XmasGift,
    XmasReindeer,
    XmasSnowman,
    XmasSanta
}

[Serializable]
public class BossTypeToPrefabDictionary : SerializableDictionary<EBoss, GameObject> { }

[CreateAssetMenu(fileName = "BossStore", menuName = "GameData/BossStore", order = 1)]
public class BossStore : ScriptableObject
{
    public BossTypeToPrefabDictionary BossTypeToPrefabDictionary;

    public GameObject GetBossPrefab(EBoss type)
    {
        if (!BossTypeToPrefabDictionary.ContainsKey(type))
            return null;

        return BossTypeToPrefabDictionary[type];
    }
}
