using JetBrains.Annotations;
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

    [UsedImplicitly]
    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void OnStartBattleButtonClick()
    {
        _menuScreenManager.GoToScreen(EScreen.Game);
    }
}
