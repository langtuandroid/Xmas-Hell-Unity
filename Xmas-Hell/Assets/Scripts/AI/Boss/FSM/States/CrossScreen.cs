using UnityEngine;

namespace BossBehaviourState
{
    public class CrossScreen : BossStateMachineBehaviour
    {
        public bool Vertical = true;
        public bool Horizontal = true;
        public string NextStateTrigger;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var gameAreaBounds = Boss.GameManager.GameArea.GetWorldRect();
            var newPosition = Boss.Position;

            if (Vertical)
            {
                // Top to bottom
                if (newPosition.y > 0)
                    newPosition.y = gameAreaBounds.yMin - Boss.Height;
                // Bottom to top
                else
                    newPosition.y = gameAreaBounds.yMax + Boss.Height;
            }

            if (Horizontal)
            {
                // Left to right
                if (newPosition.x < 0)
                    newPosition.x = gameAreaBounds.xMax + Boss.Width;
                // Right to left
                else
                    newPosition.x = gameAreaBounds.xMin - Boss.Width;
            }

            Boss.MoveTo(newPosition, null, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!Boss.TargetingPosition)
                animator.SetTrigger(NextStateTrigger);
        }
    }
}