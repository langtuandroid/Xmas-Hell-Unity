using System;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundType
{
    NONE = 0,
    SELECT = 1,
    CANCEL = 2,
    SHOOT = 3,
    BOSS_HIT = 4,
    PLAYER_DEATH = 5,
}

[Serializable]
public struct SoundClip
{
    public ESoundType soundType;
    public List<AudioClip> audioClips;
}

[CreateAssetMenu(fileName = "SoundStore", menuName = "Audio/SoundStore")]
public class SoundStore : ScriptableObject
{
    [SerializeField] private List<SoundClip> _sounds;

    public List<SoundClip> Sounds => _sounds;
}
