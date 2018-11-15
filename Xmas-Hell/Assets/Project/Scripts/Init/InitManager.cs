using UnityEngine;

public class InitManager : MonoBehaviour
{
    [SerializeField] private ScreenManager _screenManager;

    void Awake()
    {
        _screenManager.GoToScreen(EScreen.MainMenu, true);
    }

    public void ShowTransition(string transitionName = null)
    {
        ScreenTransitionManager.ShowTransition(transitionName);
    }

    public void HideTransition()
    {
        ScreenTransitionManager.HideTransition();
    }
}
