using UnityEngine;

namespace GameScreenFSM
{
    public class GameScreenStateMachineBehaviour: StateMachineBehaviour
    {
        protected GameManager GameManager;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            GameManager = GameObject.FindGameObjectWithTag("Root").GetComponent<GameManager>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}