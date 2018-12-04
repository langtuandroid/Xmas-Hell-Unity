using System;
using UnityEngine;

public enum EBoss
{
    Unknown = 0,
    Debug = 1,
    XmasBall = 2,
    XmasBell = 3,
    XmasCandy = 4,
    XmasSnowflake = 5,
    XmasLog = 6,
    XmasTree = 7,
    XmasGift = 8,
    XmasReindeer = 9,
    XmasSnowman = 10,
    XmasSanta = 11
}

public enum EBossBallState
{
    Unknown = 0,
    Available = 1,
    Beaten = 2
}

[Serializable]
public struct BossTuple
{
    public EBoss Boss1;
    public EBoss Boss2;
}

[Serializable]
public class BossRelationships : SerializableDictionary<EBoss, BossTuple> { }

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
    
    [SerializeField] private BossRelationships _bossRelationships = null;

    public BossRelationships BossRelationships => _bossRelationships;

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
