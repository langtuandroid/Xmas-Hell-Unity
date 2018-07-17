using UnityEngine;

namespace XmasBallBehaviour2FSM
{
    public class MoveToCenter : BossStateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Speed = Boss.InitialSpeed * 50f;
            Boss.MoveTo(Vector2.zero, true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            Debug.Log("Boss position: " + Boss.Position.ToString());

            if ((Mathf.Approximately(Boss.Position.x, 0) && Mathf.Approximately(Boss.Position.y, 0)) ||
                (Mathf.Abs(Boss.Position.x) < 0.5f && Mathf.Abs(Boss.Position.y) < 0.5f))
            {
                animator.SetTrigger("StartCircularPattern");
            }
        }
    }
}