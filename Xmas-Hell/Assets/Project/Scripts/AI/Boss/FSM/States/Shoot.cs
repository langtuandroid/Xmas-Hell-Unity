using UnityEngine;

namespace BossBehaviourState
{
    public class Shoot : BossStateMachineBehaviour
    {
        public string PatternName = "default";

        [Header("Collision related data")]
        public bool ShootToCollisionContactPoint = false;
        public bool UseCollisionContactNormal = false;
        public Vector2 ShootToCollisionContactNormalModifier = Vector2.one;

        [Header("Frequency")]
        public float ShootFrequency = 0f;

        private Animator _currentAnimator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _currentAnimator = animator;

            if (ShootFrequency == 0)
                ShootPattern();
            else
                Boss.StartShootTimer(ShootFrequency, ShootTimerFinished);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            if (ShootFrequency > 0)
                Boss.StopShootTimer();
        }

        private void ShootTimerFinished()
        {
            ShootPattern();
        }

        private void ShootPattern()
        {
            if (ShootToCollisionContactPoint)
            {
                Vector2 position = new Vector2(
                    _currentAnimator.GetFloat("CollisionContactPointX"),
                    _currentAnimator.GetFloat("CollisionContactPointY")
                );

                if (UseCollisionContactNormal)
                {
                    float direction = MathHelper.DirectionToAngle(new Vector2(
                        _currentAnimator.GetFloat("CollisionContactNormalX"),
                        _currentAnimator.GetFloat("CollisionContactNormalY")
                    ) * ShootToCollisionContactNormalModifier);

                    Boss.ShootPattern(PatternName, position, direction);
                }
                else
                {
                    Boss.ShootPattern(PatternName, position);
                }
            }
            else
            {
                Boss.ShootPattern(PatternName);
            }
        }
    }
}