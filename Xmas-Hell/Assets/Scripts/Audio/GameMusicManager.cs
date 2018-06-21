using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameMusicManager : MonoBehaviour
{
    public AudioSource Intro;
    public AudioSource Loop;

    private bool startedLoop = false;

    void Start()
    {
        Intro.Play();
    }

    void FixedUpdate()
    {
        if (!Intro.isPlaying && !startedLoop)
        {
            Loop.Play();
            startedLoop = true;
        }
    }
}
