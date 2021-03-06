﻿using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuScreenManager : ScreenManager
{
    public Animator MenuAnimator;
    public PlayerFrontAnimator PlayerFrontAnimator;
    public BossStore BossStore;


    [SerializeField] private BossPanel _bossPanel;

    public void Start()
    {
        Application.targetFrameRate = 60;

        if (GetPreviousScreen() == EScreen.Game)
        {
            MenuAnimator.Play("GoToBossSelection", 0, 1f);
            PlayerFrontAnimator.StopAnimation(true);
        }
    }

    public void GoToBossSelection()
    {
        MenuAnimator.SetFloat("SpeedMultiplier", 1f);
        MenuAnimator.Play("GoToBossSelection");
        PlayerFrontAnimator.StopAnimation(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuAnimator.SetFloat("SpeedMultiplier", -1f);
            MenuAnimator.Play("GoToBossSelection");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.PlaySound(ESoundType.SELECT);
        }

        // Make sure we stop the animation when we played it entirely
        if ((MenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && MenuAnimator.GetFloat("SpeedMultiplier") > 0) ||
            (MenuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && MenuAnimator.GetFloat("SpeedMultiplier") < 0))
        {
            if (MenuAnimator.GetFloat("SpeedMultiplier") < 0)
                PlayerFrontAnimator.StopAnimation(false);

            MenuAnimator.SetFloat("SpeedMultiplier", 0f);
        }
    }

    [UsedImplicitly]
    public void ShowBossPanel(EBoss bossType)
    {
        _bossPanel.gameObject.SetActive(true);
        _bossPanel.Initialize(bossType);
    }
}
