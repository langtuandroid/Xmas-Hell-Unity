using UnityEngine;

namespace XmasSnowflakeBehaviour1FSM
{
    public class RandomMovement : BossStateMachineBehaviour
    {
        public bool LongDistance = false;
        public float RandomMovementTime = 1.5f;
        public float ShootFrequency = 0.5f;

        private Animator _animator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _animator = animator;

            Boss.StartMovingRandomly(null, false, RandomMovementTime);
            Boss.StartShootTimer(ShootFrequency, ShootTimerFinished);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.StopMovingRandomly();
            Boss.StopShootTimer();
        }

        private void ShootTimerFinished()
        {
            _animator.SetTrigger("ShootTrigger");
        }
    }
}