using UnityEngine;

public class InitManager : MonoBehaviour
{
    [SerializeField] private ScreenManager _screenManager = null;
    [SerializeField] private ScreenTransitionManager _screenTransitionManager = null;
    [SerializeField] private ScreenTransitionStore _screenTransitionStore = null;

    void Awake()
    {
        PlayGamesServices.Instance.SignIn();

        _screenTransitionManager.Initialize(_screenTransitionStore);
        _screenManager.GoToScreen(EScreen.MainMenu);
    }
}
