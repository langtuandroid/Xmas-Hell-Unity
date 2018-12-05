using UnityEngine;

namespace BossBehaviourState
{
    public class Extend : BossStateMachineBehaviour
    {
        private Animator _currentAnimator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _currentAnimator = animator;

            if (_currentAnimator.GetBool("ReverseExtend"))
                _currentAnimator.SetFloat("ExtendSpeedMultiplier", -1f);
            else
                _currentAnimator.SetFloat("ExtendSpeedMultiplier", 1f);

            Boss.OnCollision.AddListener(OnCollision);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_currentAnimator.GetBool("ReverseExtend") &&
                _currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 &&
                !_currentAnimator.IsInTransition(0))
            {
                _currentAnimator.SetTrigger("ExtendFinished");
                _currentAnimator.SetFloat("ExtendSpeedMultiplier", 1f);
                _currentAnimator.SetBool("ReverseExtend", false);
            }
        }

        private void OnCollision(Collision2D collision)
        {
            if (_currentAnimator.GetBool("ReverseExtend"))
                return;

            if (collision.gameObject.tag == "Wall")
            {
                var contact = collision.GetContact(0);
                _currentAnimator.SetFloat("CollisionContactPointX", contact.point.x);
                _currentAnimator.SetFloat("CollisionContactPointY", contact.point.y);
                _currentAnimator.SetFloat("CollisionContactNormalX", contact.normal.x);
                _currentAnimator.SetFloat("CollisionContactNormalY", contact.normal.y);

                _currentAnimator.SetBool("ReverseExtend", true);

                _currentAnimator.SetTrigger("OnWallCollision");
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.OnCollision.RemoveListener(OnCollision);
        }
    }
}