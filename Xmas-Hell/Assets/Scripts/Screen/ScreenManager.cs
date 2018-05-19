using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public void GoToScreen(string screenName)
    {
        SceneManager.LoadScene(screenName);
    }
}
