using UnityEngine;

namespace BossBehaviourState
{
    public class ShootRainBullet : BossStateMachineBehaviour
    {
        public string PatternName = "default";

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Vector2 randomPosition;

            // Left or top?
            //if (Random.value > 0.5f)
            {
                // Left
                randomPosition = new Vector2(-7, Random.Range(0f, -15f));
            }
            //else
            //{
            //    // Top
            //    randomPosition = new Vector2(Random.Range(-5f, 0f), 15f);
            //}

            //randomPosition = Vector2.zero;
            Boss.ShootPattern(PatternName, randomPosition);
        }
    }
}