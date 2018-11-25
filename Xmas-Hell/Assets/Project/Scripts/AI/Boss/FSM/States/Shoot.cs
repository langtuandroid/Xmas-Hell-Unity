using UnityEngine;

namespace BossBehaviourState
{
    public class Shoot : BossStateMachineBehaviour
    {
        public string PatternName = "default";

        [Header("Frequency")]
        public float ShootFrequency = 0f;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (ShootFrequency == 0)
                Boss.ShootPattern(PatternName);
            else
                Boss.StartShootTimer(ShootFrequency, ShootTimerFinished);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            if (ShootFrequency > 0)
                Boss.StopShootTimer();
        }

        private void ShootTimerFinished()
        {
            Boss.ShootPattern(PatternName);
        }
    }
}