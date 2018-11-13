using UnityEngine;

public class WaitState : GameStateMachineBehaviour
{
    public float _timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        StartTimer(_timer);
    }
}
