using UnityEngine;

namespace XmasBallBehaviour2FSM
{
    public class MoveToCenter : BossStateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Invincible = true;
            Boss.MoveTo(Vector2.zero, 2f, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (Mathf.Abs(Boss.Position.x) < 0.5f && Mathf.Abs(Boss.Position.y) < 0.5f)
            {
                Boss.Invincible = false;
                animator.SetTrigger("StartCircularPattern");
            }
        }
    }
}