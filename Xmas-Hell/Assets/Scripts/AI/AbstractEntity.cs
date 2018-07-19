using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractEntity : MonoBehaviour
{
    public float Speed;
    public Vector2 Direction = Vector2.zero; // values in radians
    public Vector2 Acceleration = Vector2.one;
    public float AngularVelocity = 5f;
    public bool Invincible;
    public UnityEvent OnTakeDamage; 

    private Rigidbody2D _rigidbody;

    // Random moving
    private bool _movingRandomly;
    private float _randomMovementTime;
    private bool _movingLongDistance;
    protected Rect _randomMovingArea;

    // Position targeting
    private bool _targetingPosition = false;
    private Vector2 _startPosition = Vector2.zero;
    private Vector2 _targetPosition = Vector2.zero;
    private float _targetPositionTimer = 0f;
    private float _targetPositionTime = 0f;
    private Vector2 _targetDirection = Vector2.zero;

    // Angle targeting
    private bool _targetingAngle = false;
    private float _initialAngle = 0f;
    private float _targetAngle = 0f;
    private float _targetAngleTimer = 0f;
    private float _targetAngleTime = 0f;

    // Sprite
    protected Vector2 SpriteSize;

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

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (!_rigidbody)
            throw new Exception("No RigidBody2D found in this scene!");
    }

    protected virtual void Start()
    {
        ComputeSpriteSize();
    }

    protected virtual void RestoreDefaultState()
    {
        Direction = Vector2.zero;
        Rotation = 0;
        _targetingPosition = false;
        _targetingAngle = false;
        _movingRandomly = false;
        _movingLongDistance = false;
    }

    protected virtual void Update()
    {
        if (_movingRandomly)
            MoveToRandomPosition(_randomMovementTime);

        UpdatePosition();
        UpdateRotation();
    }

    private void ComputeSpriteSize()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Bounds spriteBounds = new Bounds(transform.position, Vector3.zero);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            spriteBounds.Encapsulate(spriteRenderer.bounds);

        SpriteSize = spriteBounds.min - spriteBounds.max;
        SpriteSize.x = Mathf.Abs(SpriteSize.x);
        SpriteSize.y = Mathf.Abs(SpriteSize.y);

        var dot = Resources.Load("Debug/Dot");
        Instantiate(dot, spriteBounds.min, Quaternion.identity, transform);
        Instantiate(dot, spriteBounds.max, Quaternion.identity, transform);
    }

    private void MoveToRandomPosition(float time)
    {
        if (!_targetingPosition)
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
    public void MoveTo(Vector2 position, float? time = null, bool force = false)
    {
        if (_targetingPosition && !force)
            return;

        _targetingPosition = true;
        _targetPosition = position;

        if (time.HasValue && time.Value > 0)
        {
            _startPosition = Position;

            _targetPositionTimer = time.Value;
            _targetPositionTime = time.Value;
        }
        else
        {
            _targetDirection = (position - new Vector2(Position.x, Position.y));
            _targetDirection.Normalize();
        }
    }

    public void MoveToCenter(float? time = null, bool force = false)
    {
        MoveTo(Vector2.zero, time, force);
    }

    public virtual void TakeDamage(float damage)
    {
        if (Invincible)
            return;

        OnTakeDamage.Invoke();
    }

    public void StartMovingRandomly(Rect? movingArea = null, bool longDistance = false, float time = 1.5f)
    {
        _movingRandomly = true;

        if (movingArea.HasValue)
            _randomMovingArea = movingArea.Value;

        _movingLongDistance = longDistance;
        _randomMovementTime = time;
    }

    public void StopMovingRandomly()
    {
        _movingRandomly = false;
    }

    private void UpdatePosition()
    {
        var deltaPosition = Speed * Time.deltaTime * Acceleration * Direction;

        if (_targetingPosition)
        {
            if (!_targetDirection.Equals(Vector2.zero))
            {
                var currentPosition = Position;
                var distance = Vector2.Distance(currentPosition, _targetPosition);
                var deltaDistance = Speed * Time.deltaTime;

                if (distance < deltaDistance)
                {
                    _targetingPosition = false;
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
                var lerpAmount = _targetPositionTime / _targetPositionTimer;

                newPosition.x = Mathf.SmoothStep(_targetPosition.x, _startPosition.x, lerpAmount);
                newPosition.y = Mathf.SmoothStep(_targetPosition.y, _startPosition.y, lerpAmount);

                if (lerpAmount < 0.001f)
                {
                    _targetingPosition = false;
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
        if (_targetingAngle)
        {
            // TODO: Add some logic to know if the boss has to turn to the left or to the right

            if (_targetAngleTimer <= 0)
            {
                var currentRotation = Rotation;
                var distance = Math.Abs(currentRotation - _targetAngle);
                var deltaDistance = AngularVelocity * Time.deltaTime;

                if (distance < deltaDistance)
                {
                    _targetingAngle = false;
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
                    _targetingAngle = false;
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
