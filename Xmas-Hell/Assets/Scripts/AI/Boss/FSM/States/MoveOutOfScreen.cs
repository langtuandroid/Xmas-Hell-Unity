using UnityEngine;

namespace BossBehaviourState
{
    public class MoveOutOfScreen : BossStateMachineBehaviour
    {
        public float TimeToMove;
        public string NextStateTrigger;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Invincible = true;
            Boss.MoveOutOfScreen(TimeToMove, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!Boss.TargetingPosition)
            {
                Boss.Invincible = false;
                animator.SetTrigger(NextStateTrigger);
            }
        }
    }
}