using UnityEngine;

namespace BossBehaviourState
{
    public class MoveRandomly : BossStateMachineBehaviour
    {
        public bool LongDistance = false;
        public float RandomMovementTime = 1.5f;
        public bool EnableShooting = true;
        public float ShootFrequency = 0.5f;

        [Header("Random area (normalized)")]
        public Vector2 BottomLeftCorner = Vector2.zero;
        public Vector2 TopRightCorner = Vector2.one;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.StartMovingRandomly(
                new Vector4(
                    BottomLeftCorner.x,
                    BottomLeftCorner.y,
                    TopRightCorner.x,
                    TopRightCorner.y
                ),
                LongDistance,
                RandomMovementTime
            );

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