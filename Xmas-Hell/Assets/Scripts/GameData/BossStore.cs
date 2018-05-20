﻿using System;
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
public class BossTypeToPrefab : SerializableDictionary<EBoss, GameObject>
{
}

[CreateAssetMenu(fileName = "BossStore", menuName = "GameData/BossStore", order = 1)]
public class BossStore : ScriptableObject
{
    [SerializeField]
    private BossTypePrefabDictionary BossTypeToPrefab = BossTypePrefabDictionary.New<BossTypePrefabDictionary>();


    public GameObject GetBossPrefab(EBoss type)
    {
        if (!BossTypeToPrefab.dictionary.ContainsKey(type))
            return null;

        return BossTypeToPrefab.dictionary[type];
    }
}
