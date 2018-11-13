using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    #region Serialize fields

    [SerializeField] private float _minZoom = 9.6f;
    [SerializeField] private float _maxZoom = 1f;

    #endregion

    public UnityEvent OnCameraZoomFinished = new UnityEvent();

    // Cameras
    private Camera[] _cameras;
    private float[] _camerasInitialSize;

    // Zoom
    private bool _isZooming;
    private Vector2 _zoomFocusPoint;
    private float _targetZoom;
    private float _initialZoomTimer;
    private float _zoomTimer;

    void Start()
    {
        // Get all cameras in the current GameObject children
        _cameras = GetComponentsInChildren<Camera>();
        _camerasInitialSize = new float[_cameras.Length];

        Debug.Log("Camera manager found " + _cameras.Length + " cameras!");

        _isZooming = false;
    }

    public void Reset()
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            Camera camera = _cameras[i];
            camera.orthographicSize = _camerasInitialSize[i];
        }

        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (_isZooming)
        {
            _zoomTimer -= Time.deltaTime;
            var progress = 1f - (_zoomTimer / _initialZoomTimer);

            for (int i = 0; i < _cameras.Length; i++)
            {
                Camera camera = _cameras[i];

                transform.position = Vector2.Lerp(Vector2.zero, _zoomFocusPoint, progress);
                camera.orthographicSize = Mathf.Lerp(_camerasInitialSize[i], _targetZoom, progress);
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, _maxZoom, _minZoom);
            }

            if (progress > 1f)
            {
                _isZooming = false;
                OnCameraZoomFinished.Invoke();
            }
        }
    }

    public void ZoomTo(float zoom, Vector2 focusPoint, float duration = 0f)
    {
        if (_isZooming)
            return;

        for (int i = 0; i < _cameras.Length; i++)
            _camerasInitialSize[i] = _cameras[i].orthographicSize;

        _isZooming = true;
        _zoomFocusPoint = focusPoint;
        _targetZoom = zoom;
        _initialZoomTimer = duration;
        _zoomTimer = _initialZoomTimer;
    }

    public void Shake(float duration, float magnitude)
    {
        // TODO: Implement this
    }
}
