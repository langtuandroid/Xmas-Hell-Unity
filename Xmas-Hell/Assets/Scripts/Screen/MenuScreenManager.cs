﻿using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuScreenManager : MonoBehaviour
{
    public Animator MenuAnimator;

    public void GoToBossSelection()
    {
        MenuAnimator.SetBool("IsBossSelectionScreen", true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuAnimator.SetBool("IsBossSelectionScreen", false);
        }
    }
}