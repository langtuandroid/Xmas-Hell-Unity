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
        //StartCoroutine(PlayEngineSound());
    }

    void FixedUpdate()
    {
        if (!Intro.isPlaying && !startedLoop)
        {
            Loop.Play();
            Debug.Log("Done playing");
            startedLoop = true;
        }
    }

    IEnumerator PlayEngineSound()
    {
        Intro.Play();

        yield return new WaitForSeconds(Intro.clip.length);

        Loop.Play();
    }
}
