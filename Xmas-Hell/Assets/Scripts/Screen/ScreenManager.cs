using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EScreen
{
    None,
    MainMenu,
    Game
}

public static class ScreenManager
{
    private static EScreen CurrentScreen = EScreen.MainMenu;
    private static Stack<EScreen> _previousScenes = new Stack<EScreen>();

    public static void GoToScreen(EScreen screenType)
    {
        if (CurrentScreen != EScreen.None)
            _previousScenes.Push(CurrentScreen);

        SceneManager.LoadScene(ScreenTypeToString(screenType));

        CurrentScreen = screenType;
    }

    public static void GoBack()
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
