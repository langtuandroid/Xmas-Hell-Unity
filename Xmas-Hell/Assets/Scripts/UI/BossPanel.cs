using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : MonoBehaviour
{
    public TextMeshProUGUI BossNameText;
    public Image BossBallImage; 

    public void OnEnable()
    {
        // TODO: Update data according to selected boss (from SessionData)
        BossNameText.text = SessionData.SelectedBoss == EBoss.XmasBall ? "Xmas Ball" : "[Unknow Boss]";

        if (BossBallImage && SessionData.SelectedBoss == EBoss.XmasBall)
            BossBallImage.sprite = null;
    }

    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void OnStartBattleButtonClick()
    {
        ScreenManager.GoToScreen(EScreen.Game);
    }
}
