using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScreen
{
    None,
    MainMenu,
    Game
}

public class ScreenManager : MonoBehaviour
{
    private static EScreen CurrentScreen = EScreen.MainMenu;
    private static Stack<EScreen> _previousScenes = new Stack<EScreen>();

    public void GoToScreen(EScreen screenType, bool showTransition = true)
    {
        if (showTransition)
        {
            ScreenTransitionManager.ShowTransition();
            StartCoroutine(LoadScene(screenType));
        }
        else
        {
            SceneManager.LoadScene(ScreenTypeToString(screenType));
        }

        if (CurrentScreen != EScreen.None)
            _previousScenes.Push(CurrentScreen);

        CurrentScreen = screenType;
    }

    private IEnumerator LoadScene(EScreen screenType)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(ScreenTypeToString(screenType));
    }

    public void GoBack()
    {
        if (_previousScenes.Count > 0)
            GoToScreen(_previousScenes.Pop());
    }

    public static EScreen GetPreviousScreen()
    {
        if (_previousScenes.Count > 0)
            return _previousScenes.Peek();
        else
            return EScreen.None;
    }

    private static string ScreenTypeToString(EScreen screenType)
    {
        if (screenType == EScreen.MainMenu)
            return "MenuScreen";
        else if (screenType == EScreen.Game)
            return "GameScreen";

        throw new Exception("Unknow screen type");
    }
}
