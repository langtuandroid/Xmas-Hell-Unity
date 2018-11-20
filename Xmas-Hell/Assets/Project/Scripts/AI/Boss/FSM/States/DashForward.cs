using UnityEngine;

namespace BossBehaviourState
{
    public class DashForward : BossStateMachineBehaviour
    {
        public float SpeedMultiplier = 1f;
        public Vector2 Acceleration = Vector2.zero;
        public bool Bounce = false;

        private Animator _animator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Speed *= SpeedMultiplier;
            Boss.OnCollision.AddListener(OnCollision);

            _animator = animator;

            Boss.Direction = Boss.transform.up;
        }

        private void OnCollision(Collision2D collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                if (Bounce)
                {
                    Boss.Direction = Vector2.Reflect(Boss.Direction, collision.contacts[0].normal);
                    Boss.Rotation = MathHelper.DirectionToAngle(Boss.Direction) + 180f;
                }
                else
                {
                    _animator.SetBool("OnCollision", true);
                }
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            Boss.Acceleration += Acceleration;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.OnCollision.RemoveListener(OnCollision);
            Boss.Acceleration = Vector2.one;
            Boss.Direction = Vector2.zero;
        }
    }
}