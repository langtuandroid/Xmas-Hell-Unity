using UnityEngine;

namespace BossBehaviourState
{
    public class Extend : BossStateMachineBehaviour
    {
        private bool _revertExtend;
        private Animator _currentAnimator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _revertExtend = false;
            _currentAnimator = animator;

            Boss.OnCollision.AddListener(OnCollision);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_revertExtend && _currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && 
                !_currentAnimator.IsInTransition(0))
            {
                _currentAnimator.SetTrigger("ExtendFinished");
                _currentAnimator.SetFloat("ExtendSpeedMultiplier", 1f);
            }
        }

        private void OnCollision(Collision2D collision)
        {
            if (_revertExtend)
                return;

            if (collision.gameObject.tag == "Wall")
            {
                Boss.GameManager.CameraManager.Shake(0.5f, 0.5f);
                _revertExtend = true;
                _currentAnimator.SetFloat("ExtendSpeedMultiplier", -1f);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.OnCollision.RemoveListener(OnCollision);
        }
    }
}