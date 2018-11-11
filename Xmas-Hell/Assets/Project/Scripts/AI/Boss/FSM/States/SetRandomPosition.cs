using UnityEngine;

namespace BossBehaviourState
{
    public class SetRandomPosition : BossStateMachineBehaviour
    {
        public bool Vertical = true;
        public bool Horizontal = true;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var gameAreaBounds = Boss.GameManager.GameArea.GetWorldRect();
            var newPosition = Boss.Position;

            if (Vertical)
                newPosition.y = Random.Range(gameAreaBounds.yMin + Boss.Height, gameAreaBounds.yMax - Boss.Height);
            if (Horizontal)
                newPosition.x = Random.Range(gameAreaBounds.xMin + Boss.Width, gameAreaBounds.xMax - Boss.Width);

            Boss.Position = newPosition;
        }
    }
}