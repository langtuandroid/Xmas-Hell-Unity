using System;
using System.Collections.Generic;
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

[Serializable]
public class BossTypeToBossBallDictionary : SerializableDictionary<EBoss, GameObject> { }

[CreateAssetMenu(fileName = "BossStore", menuName = "GameData/BossStore", order = 1)]
public class BossStore : ScriptableObject
{
    public BossTypeToPrefabDictionary BossTypeToPrefabDictionary;
    public BossTypeToBossBallDictionary BossTypeToBossBallSpriteDictionary;

    public GameObject GetBossPrefab(EBoss type)
    {
        if (!BossTypeToPrefabDictionary.ContainsKey(type))
            return null;

        return BossTypeToPrefabDictionary[type];
    }

    public GameObject GetBossBallSprite(EBoss type)
    {
        if (!BossTypeToBossBallSpriteDictionary.ContainsKey(type))
            return null;

        return BossTypeToBossBallSpriteDictionary[type];
    }
}
