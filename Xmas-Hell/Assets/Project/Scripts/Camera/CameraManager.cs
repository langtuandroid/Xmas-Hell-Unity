using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    #region Serialize fields

    [SerializeField] private float _minZoom = 9.6f;
    [SerializeField] private float _maxZoom = 1f;

    #endregion

    public UnityEvent OnCameraZoomInFinished = new UnityEvent();
    public UnityEvent OnCameraZoomOutFinished = new UnityEvent();

    // Cameras
    private Camera[] _cameras;
    private float[] _camerasInitialSize;

    // Zoom
    private bool _isZooming;
    private Transform _zoomFocus;
    private float _targetZoom;
    private float _initialZoomTimer;
    private float _zoomTimer;

    void Start()
    {
        // Get all cameras in the current GameObject children
        _cameras = GetComponentsInChildren<Camera>();
        _camerasInitialSize = new float[_cameras.Length];

        for (int i = 0; i < _cameras.Length; i++)
        {
            _camerasInitialSize[i] = _cameras[i].orthographicSize;
        }

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

                transform.position = Vector3.Lerp(Vector3.zero, _zoomFocus.position, progress);
                camera.orthographicSize = Mathf.Lerp(_camerasInitialSize[i], _targetZoom, progress);
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, _maxZoom, _minZoom);
            }


            if (progress > 1f)
            {
                _isZooming = false;
                OnCameraZoomInFinished.Invoke();
            }
        }
    }

    public void ZoomTo(float zoom, Transform focus, float duration = 0f)
    {
        if (_isZooming)
            return;

        _isZooming = true;
        _zoomFocus = focus;
        _targetZoom = zoom;
        _initialZoomTimer = duration;
        _zoomTimer = _initialZoomTimer;
    }

    public void Shake(float duration, float magnitude)
    {
        // TODO: Implement this
    }
}
