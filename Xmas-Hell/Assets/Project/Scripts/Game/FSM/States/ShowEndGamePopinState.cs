using UnityEngine;

public class ShowEndGamePopinState : GameStateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.ShowEndGamePanel();
    }
}
