using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : MonoBehaviour
{
    public TextMeshProUGUI BossNameText;
    public Image BossBallImage;

    private MenuScreenManager _menuScreenManager;

    public void Awake()
    {
        _menuScreenManager = GameObject.FindGameObjectWithTag("Root").GetComponent<MenuScreenManager>();
    }

    public void OnEnable()
    {
        BossNameText.text = SessionData.SelectedBoss.ToString();
        BossBallImage.sprite = _menuScreenManager.BossStore.GetBossBallSprite(SessionData.SelectedBoss, EBossBallState.Available);
    }

    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void OnStartBattleButtonClick()
    {
        _menuScreenManager.GoToScreen(EScreen.Game);
    }
}
