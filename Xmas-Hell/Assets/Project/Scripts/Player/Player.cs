﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _characterRoot;
    [SerializeField] private GameArea _gameArea;

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

    [Header("Death")]
    public UnityEvent OnPlayerDeath = new UnityEvent();
    [SerializeField] private GameObject _deathFx;

    [Header("Debug")]
    [SerializeField] private bool _debugIsInvicible = false;

    // Shoot
    private float _nextFire = 0f;
    private Queue<AbstractBullet> _bulletsPool;

    // Controls
    private Vector3 _initialTouchPosition;
    private Vector3 _initialPosition;
    private Rect _gameAreaBounds;
    private bool _isAlive;
    private int _previousTouchCount;

    private void Start()
    {
        // Bullets
        _bulletsPool = new Queue<AbstractBullet>();

        for (int i = 0; i < _bulletsPoolSize; i++)
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.SetActive(false);
            _bulletsPool.Enqueue(bullet.GetComponent<AbstractBullet>());
        }

        // Game area
        if (_gameArea != null)
            _gameAreaBounds = _gameArea.GetWorldRect();
    }

    public void Initialize()
    {
        _isAlive = true;
        _characterRoot.SetActive(true);
        _deathFx.SetActive(false);
    }

    public void Kill()
    {
        _isAlive = false;
    }

    public void Destroy()
    {
        _characterRoot.SetActive(false);

        // Trigger explosion FX
        _deathFx.SetActive(true);
    }

    void FixedUpdate()
    {
        if (!_isAlive)
            return;

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
                    var bullet = _bulletsPool.Dequeue();
                    bullet.gameObject.SetActive(true);

                    bullet.Speed = _bulletSpeed;
                    bullet.SetEmitter(gameObject);

                    bullet.transform.position = shootingPoint.transform.position;

                    var shootingPointRotation = shootingPoint.transform.localRotation.eulerAngles.z;
                    bullet.SetDirectionFromAngle(shootingPointRotation);

                    _bulletsPool.Enqueue(bullet);
                }

                if (_shootSound)
                    AudioSource.PlayClipAtPoint(_shootSound.clip, transform.position);
            }
        }
    }

    private void UpdatePosition()
    {
        // Mouse inputs seem to be taken into account on mobile
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0) || _previousTouchCount != Input.touchCount)
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

        _previousTouchCount = Input.touchCount;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player killed by " + collision.gameObject.tag);

        if (!_debugIsInvicible)
            OnPlayerDeath.Invoke();
    }
}
