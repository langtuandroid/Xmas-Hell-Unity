using UnityEngine;

namespace XmasBallBehaviour1FSM
{
    public class Shoot : BossStateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Debug.Log("Shoot bullet!");
        }
    }
}