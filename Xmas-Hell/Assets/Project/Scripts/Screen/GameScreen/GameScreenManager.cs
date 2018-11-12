using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameScreenManager : ScreenManager
{
    public void Start()
    {
        Application.targetFrameRate = 60;
    }
}
