using UnityEngine;

public class CanvasZoomManager : MonoBehaviour
{
    public Canvas Canvas;

    [Range(0.01f, 5f)]
    public float Zoom = 1f;

    // Update is called once per frame
    void LateUpdate()
    {
        Canvas.scaleFactor = Zoom;
    }
}
