using UnityEngine;

namespace BossBehaviourState
{
    public class TargetPlayer : BossStateMachineBehaviour
    {
        public float LockingTargetTime;
        public bool Reverse;
        public float AngularVelocity = 500f;

        private float _lockingTargetTimer;
        private float _originalAngularVelocity;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _lockingTargetTimer = LockingTargetTime;
            _originalAngularVelocity = Boss.AngularVelocity;
            Boss.AngularVelocity = AngularVelocity;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            var playerDirection = Boss.GetPlayerDirectionAngle();

            if (Reverse)
                playerDirection = (playerDirection + 180) % 360;

            Boss.RotateTo(playerDirection, true, true);

            if (_lockingTargetTimer <= 0 || !Boss.TargetingAngle)
                animator.SetTrigger("LockedPlayer");
            else
                _lockingTargetTimer -= Time.deltaTime;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.AngularVelocity = _originalAngularVelocity;
        }
    }
}