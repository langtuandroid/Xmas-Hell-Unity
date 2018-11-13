﻿using UnityEngine;

public class ZoomOutCameraState : GameStateMachineBehaviour
{
    public float zoomTime = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.CameraZoomOut(zoomTime);
    }
}