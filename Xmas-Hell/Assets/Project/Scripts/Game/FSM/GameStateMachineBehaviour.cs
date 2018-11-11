using UnityEngine;

public class GameStateMachineBehaviour : StateMachineBehaviour
{
    protected GameManager GameManager;

    protected Animator Animator;
    protected AnimatorStateInfo StateInfo;
    protected int LayerIndex;

    private bool _timerEnabled;
    private float _localTimer;

    private void OnDestroy()
    {
        OnStateExit(Animator, StateInfo, LayerIndex);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
        StateInfo = stateInfo;
        LayerIndex = layerIndex;

        GameManager = animator.gameObject.GetComponent<GameManager>();

        if (GameManager == null)
        {
            Debug.LogError("No GameManager found for this Behaviour.");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_timerEnabled)
        {
            _localTimer -= Time.deltaTime;

            if (_localTimer <= 0)
            {
                animator.SetTrigger("TimerFinished");
            }
        }
    }

    protected void StartTimer(float time)
    {
        _timerEnabled = true;
        _localTimer = time;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
