using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractBoss : MonoBehaviour {

    [SerializeField]
    List<AbstractBossBehaviour> Behaviours;

    public readonly EBoss BossType;

    public float Speed;
    public Vector2 Direction = Vector2.zero; // values in radians
    public Vector2 Acceleration = Vector2.one;
    public float AngularVelocity = 5f;

    public UnityEvent OnTakeDamage; 

    protected int CurrentBehaviourIndex;
    protected int PreviousBehaviourIndex;

    private Animator _animator;
    private float _initialSpeed;

    private Rigidbody2D _rigidbody;

    protected Vector2 InitialPosition;

    private GameManager _gameManager;

    // Random moving
    private bool _movingRandomly;
    private bool _movingLongDistance;
    private Rect _randomMovingArea;

    // Position targeting
    public bool TargetingPosition = false;
    private Vector2 _initialPosition = Vector2.zero;
    private Vector2 _targetPosition = Vector2.zero;
    private float _targetPositionTimer = 0f;
    private float _targetPositionTime = 0f;
    private Vector2 _targetDirection = Vector2.zero;

    // Angle targeting
    public bool TargetingAngle = false;
    private float _initialAngle = 0f;
    private float _targetAngle = 0f;
    private float _targetAngleTimer = 0f;
    private float _targetAngleTime = 0f;

    // Sprite
    private Vector2 _spriteSize;

    public Vector3 Position
    {
        get { return _rigidbody.position; }
        set { _rigidbody.MovePosition(value); }
    }

    public float Rotation
    {
        get { return _rigidbody.rotation; }
        set { _rigidbody.MoveRotation(value); }
    }

    public float InitialSpeed
    {
        get { return _initialSpeed; }
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        if (!_animator)
            throw new Exception("No Animator found on this Boss!");

        _gameManager = GetComponentInParent<GameManager>();

        if (!_gameManager)
            throw new Exception("No GameManager found on this scene!");

        _rigidbody = GetComponent<Rigidbody2D>();

        if (!_rigidbody)
            throw new Exception("No RigidBody2D found on this scene!");
    }

    private void Start()
    {
        _initialSpeed = Speed;

        ComputeSpriteSize();

        var gameArea = _gameManager.GameArea.GetWorldRect();

        InitialPosition = new Vector2(0, gameArea.yMax - (0.1f * gameArea.yMax) - _spriteSize.y / 2f);

        _randomMovingArea = gameArea;

        _randomMovingArea.x += _spriteSize.x / 2f;
        _randomMovingArea.y += _spriteSize.y / 2f;
        // We substract the entire sprite size from the width and height 
        // of the area as we move it according to the half sprite's size
        _randomMovingArea.width -= _spriteSize.x;
        _randomMovingArea.height -= _spriteSize.y;

        var dot = Resources.Load("Debug/Dot");
        Instantiate(dot, new Vector2(_randomMovingArea.x, _randomMovingArea.y), Quaternion.identity);
        Instantiate(dot, new Vector2(_randomMovingArea.x + _randomMovingArea.width, _randomMovingArea.y + _randomMovingArea.height), Quaternion.identity);

        #region Initialize behaviour
        if (Behaviours.Count > 0)
        {
            // Make sure each behaviour have access to the linked boss
            foreach (var behaviour in Behaviours)
                behaviour.Initialize(this);
        }
        #endregion

        Reset();
    }

    private void Reset()
    {
        PreviousBehaviourIndex = -1;
        CurrentBehaviourIndex = 0;

        foreach (var behaviour in Behaviours)
            behaviour.Reset();

        _movingRandomly = false;
        _movingLongDistance = false;

        transform.position = new Vector2(0, 15);
        MoveTo(InitialPosition, 1, true);
    }

    void Update()
    {
        if (_movingRandomly)
            MoveToRandomPosition(1.5f);

        UpdatePosition();
        UpdateRotation();
        UpdateBehaviour();
    }

    private void ComputeSpriteSize()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Bounds spriteBounds = new Bounds(transform.position, Vector3.zero);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            spriteBounds.Encapsulate(spriteRenderer.bounds);

        _spriteSize = spriteBounds.min - spriteBounds.max;
        _spriteSize.x = Mathf.Abs(_spriteSize.x);
        _spriteSize.y = Mathf.Abs(_spriteSize.y);

        var dot = Resources.Load("Debug/Dot");
        Instantiate(dot, spriteBounds.min, Quaternion.identity, transform);
        Instantiate(dot, spriteBounds.max, Quaternion.identity, transform);

        Debug.Log("Sprite size: " + _spriteSize);
    }

    private void MoveToRandomPosition(float time)
    {
        if (!TargetingPosition)
        {
            var newPosition = FindRandomPosition();
            MoveTo(newPosition, time);
        }
    }

    private Vector2 FindRandomPosition()
    {
        var newPosition = Vector2.zero;
        if (_movingLongDistance)
        {
            var currentPosition = Position;
            var minDistance = (_randomMovingArea.width - _randomMovingArea.x) / 2;

            // Choose a random long distance new X position
            var leftSpace = currentPosition.x - _randomMovingArea.x;
            var rightSpace = _randomMovingArea.width - currentPosition.x;

            if (leftSpace > minDistance)
            {
                if (rightSpace > minDistance)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5)
                        newPosition.x = UnityEngine.Random.Range(currentPosition.x + minDistance, currentPosition.x + minDistance + _randomMovingArea.width);
                    else
                        newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, currentPosition.x - minDistance);
                }
                else
                    newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, currentPosition.x - minDistance);
            }
            else
                newPosition.x = UnityEngine.Random.Range(currentPosition.x + minDistance, currentPosition.x + minDistance + _randomMovingArea.width);

            // minDistance only depends on the random area X and width
            if (_randomMovingArea.height - _randomMovingArea.y > minDistance)
            {
                // Choose a random long distance new Y position
                var topSpace = currentPosition.y - _randomMovingArea.y;
                var bottomSpace = _randomMovingArea.height - currentPosition.y;

                if (topSpace > minDistance)
                {
                    if (bottomSpace > minDistance)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                            newPosition.y = UnityEngine.Random.Range(currentPosition.y + minDistance, currentPosition.y + minDistance + _randomMovingArea.height);
                        else
                            newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, currentPosition.y - minDistance);
                    }
                    else
                        newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, currentPosition.y - minDistance);
                }
                else
                    newPosition.y = UnityEngine.Random.Range(currentPosition.y + minDistance, currentPosition.y + minDistance + _randomMovingArea.height);
            }
            else
                newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, _randomMovingArea.y + _randomMovingArea.height);
        }
        else
        {
            newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, _randomMovingArea.x + _randomMovingArea.width);
            newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, _randomMovingArea.y + _randomMovingArea.height);
        }

        return newPosition;
    }

    // Move to a given position in "time" seconds
    public void MoveTo(Vector2 position, float time, bool force = false)
    {
        if (TargetingPosition && !force)
            return;

        TargetingPosition = true;
        _targetPositionTimer = time;
        _targetPositionTime = time;
        _targetPosition = position;
        _initialPosition = Position;
    }

    // Move to a given position keeping the actual speed
    public void MoveTo(Vector2 position, bool force = false)
    {
        if (TargetingPosition && !force)
            return;

        TargetingPosition = true;
        _targetPosition = position;
        _targetDirection = (position - new Vector2(Position.x, Position.y));
        _targetDirection.Normalize();
    }

    private void UpdateBehaviour()
    {
        UpdateBehaviourIndex();

        if (CurrentBehaviourIndex != PreviousBehaviourIndex)
        {
            if (PreviousBehaviourIndex >= 0)
                Behaviours[PreviousBehaviourIndex].StopBehaviour();

            // TODO: Trigger signal to clear all bullets
            // TODO: Make sure we restore the initial boss state for transition

            if (Behaviours.Count > 0)
            {
                Behaviours[CurrentBehaviourIndex].StartBehaviour();
            }
        }

        if (Behaviours.Count > 0)
            Behaviours[CurrentBehaviourIndex].Step();

        PreviousBehaviourIndex = CurrentBehaviourIndex;
    }

    protected void UpdateBehaviourIndex()
    {
        if (Behaviours.Count == 0)
            return;

        if (Behaviours[CurrentBehaviourIndex].IsBehaviourEnded())
        {
            CurrentBehaviourIndex++;

            if (CurrentBehaviourIndex >= Behaviours.Count)
                Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (Behaviours.Count > CurrentBehaviourIndex)
            Behaviours[CurrentBehaviourIndex].TakeDamage(damage);

        OnTakeDamage.Invoke();
    }

    public float GetLifePercentage()
    {
        if (Behaviours.Count > CurrentBehaviourIndex)
            return Behaviours[CurrentBehaviourIndex].GetLifePercentage();

        return 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Use an enum for tag
        if (collision.gameObject.tag == "PlayerBullet")
        {
            var bullet = collision.gameObject.GetComponent<AbstractBullet>();

            if (bullet)
                TakeDamage(bullet.Damage);
        }
    }

    public void StartMovingRandomly(Rect? movingArea = null, bool longDistance = false)
    {
        _movingRandomly = true;

        if (movingArea.HasValue)
            _randomMovingArea = movingArea.Value;

        _movingLongDistance = longDistance;
    }

    public void StopMovingRandomly()
    {
        _movingRandomly = false;
    }

    private void UpdatePosition()
    {
        var deltaPosition = Speed * Time.deltaTime * Acceleration * Direction;

        if (TargetingPosition)
        {
            if (!_targetDirection.Equals(Vector2.zero))
            {
                var currentPosition = Position;
                var distance = Vector2.Distance(currentPosition, _targetPosition);
                var deltaDistance = Speed * Time.deltaTime;

                if (distance < deltaDistance)
                {
                    TargetingPosition = false;
                    _targetDirection = Vector2.zero;
                    Position = _targetPosition;
                }
                else
                {
                    // TODO: Perform some cubic interpolation
                    deltaPosition = ((_targetDirection * deltaDistance) * Acceleration);
                    var newPosition = currentPosition + new Vector3(deltaPosition.x, deltaPosition.y, 0f);
                    Position = newPosition;
                }
            }
            else
            {
                var newPosition = Position;
                var lerpAmount = (float)(_targetPositionTime / _targetPositionTimer);

                newPosition.x = Mathf.SmoothStep(_targetPosition.x, _initialPosition.x, lerpAmount);
                newPosition.y = Mathf.SmoothStep(_targetPosition.y, _initialPosition.y, lerpAmount);

                if (lerpAmount < 0.001f)
                {
                    TargetingPosition = false;
                    _targetPositionTime = 0;
                    Position = _targetPosition;
                }
                else
                    _targetPositionTime -= Time.deltaTime;

                Position = newPosition;
            }
        }
        else
        {
            var newPosition = Position + new Vector3(deltaPosition.x, deltaPosition.y, 0f);
            Position = newPosition;
        }
    }

    private void UpdateRotation()
    {
        if (TargetingAngle)
        {
            // TODO: Add some logic to know if the boss has to turn to the left or to the right

            if (_targetAngleTimer <= 0)
            {
                var currentRotation = Rotation;
                var distance = Math.Abs(currentRotation - _targetAngle);
                var deltaDistance = AngularVelocity * Time.deltaTime;

                if (distance < deltaDistance)
                {
                    TargetingAngle = false;
                    Rotation = _targetAngle;
                }
                else
                {
                    var factor = (currentRotation < _targetAngle) ? 1 : -1;
                    Rotation = currentRotation + (factor * deltaDistance);
                }
            }
            else
            {
                var lerpAmount = (float)(_targetAngleTime / _targetAngleTimer);
                var newAngle = Mathf.Lerp(_targetAngle, _initialAngle, lerpAmount);

                if (lerpAmount < 0.001f)
                {
                    TargetingAngle = false;
                    _targetAngleTimer = 0;
                    Rotation = _targetAngle;
                }
                else
                    _targetAngleTime -= Time.deltaTime;

                Rotation = newAngle;
            }
        }
    }
}
