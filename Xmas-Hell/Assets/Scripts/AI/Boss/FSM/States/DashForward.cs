using UnityEngine;

namespace BossBehaviourState
{
    public class DashForward : BossStateMachineBehaviour
    {
        public float SpeedMultiplier = 1f;
        public Vector2 Acceleration = Vector2.zero;

        private Animator _animator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Speed *= SpeedMultiplier;

            Boss.OnCollision.AddListener(OnCollision);

            _animator = animator;

            var direction = MathHelper.AngleToDirection(Boss.Rotation);
            var raycastHit = Physics2D.Raycast(Boss.Position, direction, 2300f, LayerMask.GetMask("Wall"));

            if (raycastHit.collider != null)
            {
                Boss.MoveTo(raycastHit.point, null, true);
            }
        }

        private void OnCollision(Collision2D collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                //Boss.TargetingPosition = false;
                _animator.SetBool("IsStunned", true);
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
        }
    }
}