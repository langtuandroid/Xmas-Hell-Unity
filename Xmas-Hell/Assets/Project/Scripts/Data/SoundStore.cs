using System;
using UnityEngine;

public enum ESoundType
{
    NONE = 0,
    SELECT = 1,
    CANCEL = 2,
    SHOOT = 3,
    BOSS_HIT = 4,
    PLAYER_DEATH = 5
}

[Serializable]
public class SoundTypeToAudioSourceDictionary : SerializableDictionary<ESoundType, AudioClip> { }

[CreateAssetMenu(fileName = "SoundStore", menuName = "Audio/SoundStore")]
public class SoundStore : ScriptableObject
{
    [SerializeField] private SoundTypeToAudioSourceDictionary _sounds;

    public SoundTypeToAudioSourceDictionary Sounds => _sounds;
}
