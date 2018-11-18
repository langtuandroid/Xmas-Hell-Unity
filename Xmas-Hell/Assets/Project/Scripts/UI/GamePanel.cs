using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    public TextMeshProUGUI Text;
    private GameScreenManager _gameScreenManager;

    public void Awake()
    {
        _gameScreenManager = GameObject.FindGameObjectWithTag("Root").GetComponent<GameScreenManager>();
    }

    public void Initialize(bool win)
    {
        // TODO: Check the end game state from the GameManager and update the text
        if (win)
            Text.text = "You won, congratulation!";
        else
            Text.text = "You lose, try again...";
    }

    [UsedImplicitly]
    public void OnCloseButtonClick()
    {
        _gameScreenManager.GoToScreen(EScreen.MainMenu);
    }

    [UsedImplicitly]
    public void OnRetryButtonClick()
    {
        _gameScreenManager.GoToScreen(EScreen.Game);
    }
}
