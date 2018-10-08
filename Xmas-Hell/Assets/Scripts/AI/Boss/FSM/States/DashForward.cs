using UnityEngine;
using UnityEngine.Animations;

namespace BossBehaviourState
{
    public class DashForward : BossStateMachineBehaviour
    {
        public float SpeedMultiplier = 1f;
        public Vector2 Acceleration = Vector2.zero;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Speed *= SpeedMultiplier;

            var wallLayerMask = LayerMask.NameToLayer("Wall");
            var direction = MathHelper.AngleToDirection(Boss.Rotation);
            var raycastHit = Physics2D.Raycast(Boss.Position, direction, 2300f, LayerMask.GetMask("Wall"));

            if (raycastHit.collider != null)
            {
                Boss.MoveTo(raycastHit.point, null, true);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.Rotation = 0;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (Boss.TargetingPosition)
            {
                Boss.Acceleration += Acceleration;
            }
            else
            {
                animator.SetBool("IsStunned", true);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);

            Boss.Acceleration = Vector2.one;
        }
    }
}