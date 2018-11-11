using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    public float MoveSensitivity = 1f;
    public Rigidbody2D Rigidbody;

    [SerializeField] private GameArea _gameArea;

    private Vector3 _initialTouchPosition;
    private Vector3 _initialPosition;
    private Rect _gameAreaBounds;

    private void Start()
    {
        if (_gameArea != null)
        {
            _gameAreaBounds = _gameArea.GetWorldRect();
        }
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Mouse inputs seem to be taken into account on Android
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialPosition = transform.position;
                _initialTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var deltaPosition = currentMousePosition - _initialTouchPosition;

            var newPosition = _initialPosition + (deltaPosition * MoveSensitivity);
            newPosition.z = _initialPosition.z; // Make sure we don't alter the Z position

            // Make sure the player stay in the game area
            if (_gameArea != null)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, _gameAreaBounds.xMin, _gameAreaBounds.xMax);
                newPosition.y = Mathf.Clamp(newPosition.y, _gameAreaBounds.yMin, _gameAreaBounds.yMax);
            }

            Rigidbody.MovePosition(newPosition);
        }
    }
}
