using UnityEngine;

namespace BossBehaviourState
{
    public class ShootRainBullet : BossStateMachineBehaviour
    {
        public string PatternName = "default";
        public float BulletsPerWave = 5;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Vector2 randomPosition;

            for (int i = 0; i < BulletsPerWave; i++)
            {
                // Left or top?
                if (Random.value > 0.5f)
                {
                    // Left
                    randomPosition = new Vector2(-7, Random.Range(-8f, 12f));
                }
                else
                {
                    // Top
                    randomPosition = new Vector2(Random.Range(-7f, 4f), 10f);
                }

                Boss.ShootPattern(PatternName, randomPosition);
            }
        }
    }
}