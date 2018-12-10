using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundStore _soundStore;

    private static SoundManager _instance;
    private AudioSource _audioSource;

    public static SoundManager Instance => _instance;

    private void Start()
    {
        if (_instance == null)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();

            if (!_audioSource)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySound(ESoundType soundType)
    {
        var soundClip = _soundStore.Sounds.FirstOrDefault(s => s.soundType == soundType);

        if (soundClip.audioClips.Count > 0)
        {
            _audioSource.PlayOneShot(soundClip.audioClips[Random.Range(0, soundClip.audioClips.Count - 1)]);
        }
        else
        {
            Debug.LogError($"No sound found for {{soundType}}");
        }
    }

    public void PlaySelectSound()
    {
        PlaySound(ESoundType.SELECT);
    }

    public void PlayCancelSound()
    {
        PlaySound(ESoundType.CANCEL);
    }
}
