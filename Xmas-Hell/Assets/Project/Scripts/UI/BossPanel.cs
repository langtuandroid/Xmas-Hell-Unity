﻿using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : MonoBehaviour
{
    #region Serialize fields

    [Header("UI references")]
    [SerializeField] private Image _bossBallImage;
    [SerializeField] private TextMeshProUGUI _bossNameText;
    [SerializeField] private TextMeshProUGUI _bestTimeText;
    [SerializeField] private TextMeshProUGUI _playTimeText;
    [SerializeField] private TextMeshProUGUI _playerDeathsText;
    [SerializeField] private TextMeshProUGUI _bossDeathsText;

    #endregion

    private MenuScreenManager _menuScreenManager;

    public void Awake()
    {
        _menuScreenManager = GameObject.FindGameObjectWithTag("Root").GetComponent<MenuScreenManager>();
    }

    public void Initialize(EBoss bossType)
    {
        if (_bossNameText)
            _bossNameText.text = SessionData.SelectedBoss.ToString();

        _bossBallImage.sprite = _menuScreenManager.BossStore.GetBossBallSprite(SessionData.SelectedBoss, EBossBallState.Available);

        // Get boss data from player save
        var bossData = SaveSystem.GetBossData(bossType);

        if (bossData.BestTime > 0 && bossData.WinCounter > 0)
            _bestTimeText.text = TimeSpan.FromMilliseconds(bossData.BestTime).ToString("mm\\:ss\\.fff");

        var totalTime = TimeSpan.FromSeconds(bossData.TotalTime);
        if (totalTime.TotalMinutes >= 100)
            _playTimeText.text = $"{totalTime.TotalMinutes}: {totalTime.Seconds}";
        else
            _playTimeText.text = totalTime.ToString("mm\\:ss");

        _playerDeathsText.text = bossData.LoseCounter.ToString();
        _bossDeathsText.text = bossData.WinCounter.ToString();
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
