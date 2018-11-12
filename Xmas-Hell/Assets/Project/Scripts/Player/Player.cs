using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;

    [Header("Events")]
    public UnityEvent OnPlayerDeath = new UnityEvent();

    [Header("Shoot")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _fireRate = 0.05f;
    [SerializeField] private float _bulletSpeed = 10;
    [SerializeField] private List<Transform> _shootingPoints;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private int _bulletsPoolSize;

    [Header("Controls")]
    [SerializeField] private float _moveSensitivity = 1f;
    [SerializeField] private Rigidbody2D _rigidbody;

    // Shoot
    private float _nextFire = 0f;
    private Queue<GameObject> _bulletsPool;

    // Controls
    private Vector3 _initialTouchPosition;
    private Vector3 _initialPosition;
    private Rect _gameAreaBounds;
    private bool _disabled;

    private void Start()
    {
        // Bullets
        _bulletsPool = new Queue<GameObject>();

        for (int i = 0; i < _bulletsPoolSize; i++)
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.SetActive(false);
            _bulletsPool.Enqueue(bullet);
        }

        // Game area
        if (_gameArea != null)
            _gameAreaBounds = _gameArea.GetWorldRect();

        _disabled = false;
    }

    void FixedUpdate()
    {
        UpdatePosition();

        UpdateShoot();
    }

    private void UpdateShoot()
    {
        if (_shootingPoints.Count > 0)
        {
            if (Input.GetMouseButton(0) && Time.time > _nextFire)
            {
                _nextFire = Time.time + _fireRate;

                foreach (var shootingPoint in _shootingPoints)
                {
                    var playerBulletObject = _bulletsPool.Dequeue();
                    playerBulletObject.SetActive(true);
                    var bulletScript = playerBulletObject.GetComponent<AbstractBullet>();

                    bulletScript.Speed = _bulletSpeed;
                    bulletScript.SetEmitter(gameObject);

                    playerBulletObject.transform.position = shootingPoint.transform.position;

                    // TODO: Should be computed only once in the Start method
                    var shootingPointRotation = shootingPoint.transform.localRotation.eulerAngles.z;
                    bulletScript.SetDirectionFromAngle(shootingPointRotation);

                    _bulletsPool.Enqueue(playerBulletObject);
                }

                if (_shootSound)
                    AudioSource.PlayClipAtPoint(_shootSound.clip, transform.position);
            }
        }
    }

    private void UpdatePosition()
    {
        if (_disabled)
            return;

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

            var newPosition = _initialPosition + (deltaPosition * _moveSensitivity);
            newPosition.z = _initialPosition.z; // Make sure we don't alter the Z position

            // Make sure the player stay in the game area
            if (_gameArea != null)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, _gameAreaBounds.xMin, _gameAreaBounds.xMax);
                newPosition.y = Mathf.Clamp(newPosition.y, _gameAreaBounds.yMin, _gameAreaBounds.yMax);
            }

            _rigidbody.MovePosition(newPosition);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player killed by " + collision.gameObject.tag);

        OnPlayerDeath.Invoke();
    }
}
