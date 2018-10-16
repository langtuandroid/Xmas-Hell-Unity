using UnityEngine;

namespace BossBehaviourState
{
    public class TargetRandomDirection : BossStateMachineBehaviour
    {
        public string NextTriggerName;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            float randomAngle = Random.Range(0f, 360f);
            Boss.RotateTo(randomAngle, true, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!Boss.TargetingAngle)
            {
                animator.SetTrigger(NextTriggerName);
            }
        }
    }
}