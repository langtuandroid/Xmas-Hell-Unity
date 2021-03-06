﻿using UnityEngine;

public class BossStateMachineBehaviour : StateMachineBehaviour
{
    protected AbstractBoss Boss;

    protected Animator Animator;
    protected AnimatorStateInfo StateInfo;
    protected int LayerIndex;

    private void OnDestroy()
    {
        OnStateExit(Animator, StateInfo, LayerIndex);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
        StateInfo = stateInfo;
        LayerIndex = layerIndex;

        Boss = animator.transform.parent.gameObject.GetComponent<AbstractBoss>();

        if (Boss == null)
        {
            Debug.LogError("No Boss found for this Behaviour.");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
