﻿using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class MenuDebugOverlay : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private TextMeshProUGUI _fpsCounter;

    // FPS counter
    private float _deltaTime = 0.0f;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // FPS counter
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        _fpsCounter.text = string.Format("{0:0.} FPS ({1:0.0}ms)", fps, msec);
    }

    [UsedImplicitly]
    public void OnGPGSSignInButtonClick()
    {
        PlayGamesServices.Instance.SignIn();
    }

    [UsedImplicitly]
    public void OnGPGSSignOutButtonClick()
    {
        PlayGamesServices.Instance.SignOut();
    }

    [UsedImplicitly]
    public void OnWinXmasBallButtonClicked()
    {
        SaveSystem.BossWon(EBoss.XmasBall, 320f);
    }

    [UsedImplicitly]
    public void OnWinXmasSnowflakeButtonClicked()
    {
        SaveSystem.BossWon(EBoss.XmasSnowflake, 320f);
    }

    [UsedImplicitly]
    public void OnWinBossButtonClicked(EBoss bossType)
    {
        SaveSystem.BossWon(bossType, 320f);
    }

    [UsedImplicitly]
    public void OnLoseXmasBallButtonClicked()
    {
        SaveSystem.BossLost(EBoss.XmasBall, 53f);
    }
}
