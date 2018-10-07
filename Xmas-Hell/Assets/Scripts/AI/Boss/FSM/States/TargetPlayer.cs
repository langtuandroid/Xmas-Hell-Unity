using UnityEngine;

namespace BossBehaviourState
{
    public class TargetPlayer : BossStateMachineBehaviour
    {
        public float LockingTargetTime;
        public bool Reverse;

        private float _lockingTargetTimer;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _lockingTargetTimer = LockingTargetTime;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            var playerDirection = Boss.GetPlayerDirectionAngle();

            if (Reverse)
                playerDirection = (playerDirection + 180) % 360;

            Boss.RotateTo(playerDirection, true);

            if (_lockingTargetTimer <= 0 || !Boss.TargetingAngle)
                animator.SetTrigger("LockedPlayer");
            else
                _lockingTargetTimer -= Time.deltaTime;
        }
    }
}