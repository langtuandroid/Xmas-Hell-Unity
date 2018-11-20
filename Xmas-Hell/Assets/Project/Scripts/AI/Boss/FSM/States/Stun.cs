using UnityEngine;
using UnityEngine.Animations;

namespace BossBehaviourState
{
    public class Stun : BossStateMachineBehaviour
    {
        public float MinTime = 1f;
        public float MaxTime = 2f;

        private float _timer;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _timer = Random.Range(MinTime, MaxTime);
            animator.SetBool("IsStunned", true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_timer < 0)
            {
                animator.SetBool("IsStunned", false);
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);
        }
    }
}