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

public enum EBossBallState
{
    Unknown,
    Available,
    Beaten
}

[Serializable]
public class BossTypeToGameObjectDictionary : SerializableDictionary<EBoss, GameObject> { }

[Serializable]
public class BossTypeToSpriteDictionary : SerializableDictionary<EBoss, Sprite> { }

[CreateAssetMenu(fileName = "BossStore", menuName = "GameData/BossStore", order = 1)]
public class BossStore : ScriptableObject
{
    public BossTypeToGameObjectDictionary BossTypeToPrefabDictionary;
    public BossTypeToSpriteDictionary BossTypeToBossBallAvailableSpriteDictionary;
    public BossTypeToSpriteDictionary BossTypeToBossBallBeatenSpriteDictionary;
    public Sprite BossBallUnknownSprite;

    public GameObject GetBossPrefab(EBoss type)
    {
        if (!BossTypeToPrefabDictionary.ContainsKey(type))
            return null;

        return BossTypeToPrefabDictionary[type];
    }

    public Sprite GetBossBallSprite(EBoss type, EBossBallState spriteType)
    {
        switch (spriteType)
        {
            case EBossBallState.Available:
                if (BossTypeToBossBallAvailableSpriteDictionary.ContainsKey(type))
                    return BossTypeToBossBallAvailableSpriteDictionary[type];
                break;
            case EBossBallState.Beaten:
                if (BossTypeToBossBallBeatenSpriteDictionary.ContainsKey(type))
                    return BossTypeToBossBallBeatenSpriteDictionary[type];
                break;
            default:
                return BossBallUnknownSprite;
        }

        return BossBallUnknownSprite;
    }
}
