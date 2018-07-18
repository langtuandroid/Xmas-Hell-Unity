using UnityEngine;

namespace XmasSnowflakeBehaviour2FSM
{
    public class MoveToPosition : BossStateMachineBehaviour
    {
        [Range(0f, 1f)]
        public float NormalizedPositionX;
        [Range(0f, 1f)]
        public float NormalizedPositionY;
        public float TimeToMove;

        private Vector2 _worldPosition;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Invincible = true;

            _worldPosition = Boss.GameManager.GameArea.NormalizedToWorldPoint(new Vector2(NormalizedPositionX, NormalizedPositionY));

            Boss.MoveTo(_worldPosition, TimeToMove, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (Mathf.Abs(Boss.Position.x - _worldPosition.x) < 0.5f && Mathf.Abs(Boss.Position.y - _worldPosition.y) < 0.5f)
            {
                Boss.Invincible = false;
                animator.SetTrigger("DetachBranches");
            }
        }
    }
}