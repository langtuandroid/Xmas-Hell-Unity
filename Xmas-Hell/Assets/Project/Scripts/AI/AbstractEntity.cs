using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractEntity : MonoBehaviour
{
    public float Speed = 1f;
    public Vector2 Direction = Vector2.zero; // values in radians
    public Vector2 Acceleration = Vector2.one;
    protected float _angularVelocity = 100f;
    public bool Invincible;
    public UnityEvent OnTakeDamage;

    protected GameManager _gameManager;

    private Rigidbody2D _rigidbody;
    private bool _isAlive;

    // Random moving
    private bool _movingRandomly;
    private float _randomMovementTime;
    private bool _movingLongDistance;
    private Rect _randomMovingArea;

    // Position targeting
    [HideInInspector]
    public bool TargetingPosition = false;
    private Vector2 _startPosition = Vector2.zero;
    private Vector2 _targetPosition = Vector2.zero;
    private float _targetPositionTimer = 0f;
    private float _targetPositionTime = 0f;
    private Vector2 _targetDirection = Vector2.zero;

    // Angle targeting
    [HideInInspector]
    public bool TargetingAngle = false;
    private readonly float _initialAngle = 0f;
    private float _targetAngle = 0f;
    private float _targetAngleTimer = 0f;
    private float _targetAngleTime = 0f;
    private int _previousRotationDirection = 0; // -1 left, 1 right
    private bool _keepRotationDirection = false;

    // Sprite
    protected Vector2 SpriteSize;
    private SpriteRenderer[] _spriteRenderers;

    // Color blink
    public float DamageBlinkTime = 0.2f;
    private float _damageBlinkTimer;
    private Color _damageColor = new Color(0f, 0f, 0f, 1f);
    private bool _tookDamage = false;

    // Debug
    List<GameObject> _debugDots = new List<GameObject>();

    public Vector3 Position
    {
        get { return Rigidbody.position; }
        set { Rigidbody.position = value; }
    }

    public float Rotation
    {
        get { return Rigidbody.rotation; }
        set { Rigidbody.rotation = value; }
    }

    public float AngularVelocity
    {
        get { return _angularVelocity; }
        set { _angularVelocity = value; }
    }

    public float Width
    {
        get { return SpriteSize.x; }
    }

    public float Height
    {
        get { return SpriteSize.y; }
    }

    public Rigidbody2D Rigidbody
    {
        get { return _rigidbody; }
    }

    public GameManager GameManager
    {
        get { return _gameManager; }
    }

    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (!Rigidbody)
            throw new Exception("No RigidBody2D found in this scene!");

        _gameManager = GameObject.FindGameObjectWithTag("Root").GetComponent<GameManager>();

        if (!_gameManager)
            throw new Exception("No GameManager found in this scene!");
    }

    protected virtual void Start()
    {
        IsAlive = true;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        ComputeSpriteSize();
    }

    protected virtual void RestoreDefaultState()
    {
        Direction = Vector2.zero;
        Rotation = 0;
        TargetingPosition = false;
        TargetingAngle = false;
        _movingRandomly = false;
        _movingLongDistance = false;
    }

    protected virtual void Reset()
    {
        _isAlive = true;
    }

    public virtual void Kill()
    {
        _isAlive = false;
    }

    protected virtual void Update()
    {
        if (!_isAlive)
            return;

        if (_movingRandomly)
            MoveToRandomPosition(_randomMovementTime);

        if (_tookDamage)
        {
            if (_damageBlinkTimer > 0)
            {
                _damageBlinkTimer -= Time.deltaTime;
            }
            else
            {
                SetColor(Color.white);
                _tookDamage = false;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    public void SetColor(Color color)
    {
        foreach (var spriteRenderer in _spriteRenderers)
            spriteRenderer.material.SetColor("_Color", color);
    }

    private void ComputeSpriteSize()
    {
        Bounds spriteBounds = new Bounds(transform.position, Vector3.zero);

        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            spriteBounds.Encapsulate(spriteRenderer.bounds);

        SpriteSize = spriteBounds.min - spriteBounds.max;
        SpriteSize.x = Mathf.Abs(SpriteSize.x);
        SpriteSize.y = Mathf.Abs(SpriteSize.y);

        //var dot = Resources.Load("Debug/Dot");
        //Instantiate(dot, spriteBounds.min, Quaternion.identity, transform);
        //Instantiate(dot, spriteBounds.max, Quaternion.identity, transform);
    }

    private void MoveToRandomPosition(float time)
    {
        if (!TargetingPosition)
        {
            var newPosition = FindRandomPosition();
            MoveTo(newPosition, time);
        }
    }
    public void MoveOutOfScreen(float? time = null, bool force = true)
    {
        MoveTo(GetNearestOutOfScreenPosition(), time, force);
    }

    private Vector2 GetNearestOutOfScreenPosition()
    {
        var outOfScreenPosition = Position;
        EScreenSide side = GetNearestBorder();
        var gameAreaBounds = _gameManager.GameArea.GetWorldRect();

        switch (side)
        {
            case EScreenSide.Left:
                outOfScreenPosition.x = gameAreaBounds.xMin - Width;
                break;
            case EScreenSide.Right:
                outOfScreenPosition.x = gameAreaBounds.xMax + Width;
                break;
            case EScreenSide.Top:
                outOfScreenPosition.y = gameAreaBounds.yMax + Height;
                break;
            case EScreenSide.Bottom:
                outOfScreenPosition.y = gameAreaBounds.yMin - Height;
                break;
        }

        return outOfScreenPosition;
    }

    public EScreenSide GetNearestBorder()
    {
        var gameAreaBounds = _gameManager.GameArea.GetWorldRect();

        // Left part
        if (Position.x < 0)
        {
            if (Position.y < 0)
            {
                if (Position.y - gameAreaBounds.yMin < Position.x - gameAreaBounds.xMin)
                    return EScreenSide.Bottom;
            }
            else
            {
                if (gameAreaBounds.yMax - Position.y < Position.x - gameAreaBounds.xMin)
                    return EScreenSide.Top;
            }

            return EScreenSide.Left;
        }
        // Right part
        else
        {
            if (Position.y < 0)
            {
                if (Position.y - gameAreaBounds.yMin < gameAreaBounds.xMax - Position.x)
                    return EScreenSide.Bottom;
            }
            else
            {
                if (gameAreaBounds.yMax - Position.y < gameAreaBounds.xMax - Position.x)
                {
                    return EScreenSide.Top;
                }
            }

            return EScreenSide.Right;
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
        if (TargetingPosition && !force)
            return;

        TargetingPosition = true;
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

    public void RotateTo(float angle, bool force = false, bool keepDirection = false)
    {
        if (TargetingAngle && !force)
            return;

        TargetingAngle = true;
        _targetAngle = angle;
        _keepRotationDirection = keepDirection;
    }

    public virtual void TakeDamage(float damage)
    {
        if (Invincible)
            return;

        if (!_tookDamage)
        {
            _tookDamage = true;
            _damageBlinkTimer = DamageBlinkTime;
            SetColor(_damageColor);
        }

        OnTakeDamage.Invoke();
    }

    public void StartMovingRandomly(Vector4? normalizedMovingArea = null, bool longDistance = false, float time = 1.5f)
    {
        _movingRandomly = true;

        if (normalizedMovingArea.HasValue)
            UpdateRandomMovingArea(normalizedMovingArea.Value);

        _movingLongDistance = longDistance;
        _randomMovementTime = time;
    }

    protected void UpdateRandomMovingArea(Vector4 movingArea, bool stayInScreen = true)
    {
        // Normalized position
        var bottomLeftCorner = new Vector2(Mathf.Clamp01(movingArea.x), Mathf.Clamp01(movingArea.y));
        var topRightCorner = new Vector2(Mathf.Clamp01(movingArea.z), Mathf.Clamp01(movingArea.w));

        var gameAreaBounds = _gameManager.GameArea.GetWorldRect();

        var min = gameAreaBounds.min;
        var max = gameAreaBounds.max;

        if (stayInScreen)
        {
            min += SpriteSize / 2f;
            max -= SpriteSize / 2f;
        }

        var size = max - min;

        min = min + size * bottomLeftCorner;
        max = max + -size * (Vector2.one - topRightCorner);

        _randomMovingArea = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

        for (int i = 0; i < _debugDots.Count; i++)
            Destroy(_debugDots[i]);

        _debugDots.Clear();

        var dot = Resources.Load("Debug/Dot");

        _debugDots.Add((GameObject)Instantiate(dot, new Vector2(_randomMovingArea.x, _randomMovingArea.y), Quaternion.identity));
        _debugDots.Add((GameObject)Instantiate(dot, new Vector2(_randomMovingArea.x + _randomMovingArea.width, _randomMovingArea.y + _randomMovingArea.height), Quaternion.identity));
    }

    public void StopMovingRandomly()
    {
        _movingRandomly = false;
    }

    private void UpdatePosition()
    {
        var deltaPosition = Speed * Time.fixedDeltaTime * Acceleration * Direction;

        if (TargetingPosition)
        {
            if (!_targetDirection.Equals(Vector2.zero))
            {
                var currentPosition = Position;
                var distance = Vector2.Distance(currentPosition, _targetPosition);
                var deltaDistance = Speed * Time.fixedDeltaTime * Acceleration;

                if (distance < deltaDistance.magnitude)
                {
                    TargetingPosition = false;
                    _targetDirection = Vector2.zero;
                    Position = _targetPosition;
                }
                else
                {
                    // TODO: Perform some cubic interpolation
                    deltaPosition = _targetDirection * deltaDistance;
                    var newPosition = currentPosition + new Vector3(deltaPosition.x, deltaPosition.y, Position.z);
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
                    TargetingPosition = false;
                    _targetPositionTime = 0;
                    Position = _targetPosition;
                }
                else
                    _targetPositionTime -= Time.fixedDeltaTime;

                Position = newPosition;
            }
        }
        else
        {
            var newPosition = Position + new Vector3(deltaPosition.x, deltaPosition.y, Position.z);
            Position = newPosition;
        }
    }

    private void UpdateRotation()
    {
        if (TargetingAngle)
        {
            if (_targetAngleTimer <= 0)
            {
                var currentRotation = Rotation;

                // Compute left and right distance to know if 
                // the boss has to turn to the left or to the right
                var leftDistance = 0f;
                var rightDistance = 0f;

                if (_targetAngle < currentRotation)
                {
                    leftDistance = 360 - currentRotation + _targetAngle;
                    rightDistance = currentRotation - _targetAngle;
                }
                else
                {
                    leftDistance = _targetAngle - currentRotation;
                    rightDistance = 360 - _targetAngle + currentRotation;
                }

                if (!_keepRotationDirection || (_keepRotationDirection && _previousRotationDirection == 0))
                    _previousRotationDirection = leftDistance < rightDistance ? 1 : -1;

                var distance = Mathf.Min(leftDistance, rightDistance);
                var deltaDistance = _angularVelocity * Time.fixedDeltaTime;

                if (distance < deltaDistance)
                {
                    TargetingAngle = false;
                    Rotation = _targetAngle;
                    _previousRotationDirection = 0;
                }
                else
                {
                    Rotation = MathHelper.WrapAngle(currentRotation + (_previousRotationDirection * deltaDistance));
                }
            }
            //else
            //{
            //    var lerpAmount = _targetAngleTime / _targetAngleTimer;
            //    var newAngle = Mathf.Lerp(_targetAngle, _initialAngle, lerpAmount);

            //    if (lerpAmount < 0.001f)
            //    {
            //        TargetingAngle = false;
            //        _targetAngleTimer = 0;
            //        Rotation = _targetAngle;
            //    }
            //    else
            //        _targetAngleTime -= Time.fixedDeltaTime;

            //    Rotation = newAngle;
            //}
        }
    }
}
