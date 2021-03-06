﻿using UnityEngine;

public class PlayerExplosionState : GameStateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.OnPlayerExplosion();
    }
}
