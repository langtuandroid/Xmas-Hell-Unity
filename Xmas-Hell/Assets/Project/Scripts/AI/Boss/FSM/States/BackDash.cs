using UnityEngine;

namespace BossBehaviourState
{
    public class BackDash : BossStateMachineBehaviour
    {
        public Vector2 InitialAcceleration = Vector2.zero;
        public Vector2 Deceleration = Vector2.zero;
        public Vector2 StopDecelerationThreshold = Vector2.zero;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Boss.Acceleration = InitialAcceleration;
            Boss.Direction = -Boss.transform.up;

            Debug.Log("Boss current speed: " + Boss.Speed);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            Boss.Acceleration -= Deceleration * Time.deltaTime;

            if (Boss.Acceleration.x <= StopDecelerationThreshold.x && Boss.Acceleration.y <= StopDecelerationThreshold.y)
                animator.SetTrigger("FinishedBackDash");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Boss.Acceleration = Vector2.zero;
        }
    }
}