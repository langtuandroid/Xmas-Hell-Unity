using UnityEngine;

namespace BossBehaviourState
{
    public class RotateShoot : BossStateMachineBehaviour
    {
        public float RandomTimerMin = 1f;
        public float RandomTimerMax = 5f;
        public float AngularVelocity = 90f;
        public string NextStateTrigger;

        private float _timer;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _timer = Random.Range(RandomTimerMin, RandomTimerMax);
            Boss.Rigidbody.angularVelocity = AngularVelocity;

            // TODO: Shoot pattern
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.Rigidbody.angularVelocity = 0f;
            Boss.Rigidbody.rotation = 0f;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            _timer -= Time.deltaTime;

            if (_timer < 0)
                animator.SetTrigger(NextStateTrigger);
        }
    }
}