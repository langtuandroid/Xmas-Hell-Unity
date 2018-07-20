using UnityEngine;

namespace BossBehaviourState
{
    public class RandomMovement : BossStateMachineBehaviour
    {
        public bool LongDistance = false;
        public float RandomMovementTime = 1.5f;
        public bool EnableShooting = true;
        public float ShootFrequency = 0.5f;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.StartMovingRandomly(null, false, RandomMovementTime);

            if (EnableShooting)
                Boss.StartShootTimer(ShootFrequency, ShootTimerFinished);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.StopMovingRandomly();

            if (EnableShooting)
                Boss.StopShootTimer();
        }

        private void ShootTimerFinished()
        {
            Animator.SetTrigger("ShootTrigger");
        }
    }
}