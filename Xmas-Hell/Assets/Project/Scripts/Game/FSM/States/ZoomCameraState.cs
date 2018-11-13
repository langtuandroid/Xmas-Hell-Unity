using UnityEngine;

public class ZoomCameraState : GameStateMachineBehaviour
{
    public float zoomOutTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.CameraZoomOut(zoomOutTime);
    }
}
