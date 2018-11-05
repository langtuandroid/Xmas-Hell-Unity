using UnityEngine;

namespace BossBehaviourState
{
    public class Shoot : BossStateMachineBehaviour
    {
        public string PatternName = "default";

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.ShootPattern(PatternName);
        }
    }
}