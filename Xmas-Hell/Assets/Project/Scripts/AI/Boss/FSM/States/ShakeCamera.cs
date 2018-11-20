using UnityEngine;

namespace BossBehaviourState
{
    public class ShakeCamera : BossStateMachineBehaviour
    {
        public float Duration = 0.5f;
        public float Magnitude = 1f;

        private Animator _animator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.GameManager.CameraManager.Shake(Duration, Magnitude);
        }
    }
}