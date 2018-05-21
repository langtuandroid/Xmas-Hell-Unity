using UnityEngine.SceneManagement;

public static class ScreenManager
{
    public static void GoToScreen(string screenName)
    {
        SceneManager.LoadScene(screenName);
    }
}
