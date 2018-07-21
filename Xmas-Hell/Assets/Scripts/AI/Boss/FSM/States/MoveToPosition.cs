using UnityEngine;

namespace BossBehaviourState
{
    public class MoveToPosition : BossStateMachineBehaviour
    {
        [Range(0f, 1f)]
        public float PositionX;
        [Range(0f, 1f)]
        public float PositionY;
        public float TimeToMove;
        public string NextStateTrigger;

        private Vector2 _worldPosition;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Invincible = true;

            _worldPosition = Boss.GameManager.GameArea.NormalizedToWorldPoint(new Vector2(PositionX, PositionY));

            Boss.MoveTo(_worldPosition, TimeToMove, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!Boss.TargetingPosition)
            {
                Boss.Invincible = false;
                animator.SetTrigger(NextStateTrigger);
            }
        }
    }
}