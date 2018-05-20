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

    public Vector3 Position
    {
        get { return gameObject.transform.position; }
        set { gameObject.transform.position = value; }
    }

    public float Rotation
    {
        get { return gameObject.transform.eulerAngles.z; }
        set {
            var newAngles = gameObject.transform.eulerAngles;
            newAngles.z = value;
            gameObject.transform.eulerAngles = newAngles;
        }
    }

    public float InitialSpeed
    {
        get { return _initialSpeed; }
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    public AbstractBoss()
    {
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        if (!_animator)
            throw new System.Exception("No Animator found on this Boss");
    }

    private void Start()
    {
        _initialSpeed = Speed;

        #region Initialize behaviour
        PreviousBehaviourIndex = -1;
        CurrentBehaviourIndex = 0;

        if (Behaviours.Count > 0)
        {
            // Make sure each behaviour have access to the linked boss
            foreach (var behaviour in Behaviours)
                behaviour.Initialize(this);
        }
        #endregion

        _movingRandomly = false;
        _movingLongDistance = false;

        // TODO: Should depends on the camera/game area data
        _randomMovingArea = new Rect(-4, 8, 4, 7.5f);
    }

    void Update()
    {

        if (_movingRandomly)
            FindRandomPosition();

        UpdatePosition();
        UpdateRotation();
        UpdateBehaviour();
    }

    private void FindRandomPosition()
    {
        if (!TargetingPosition)
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
                            newPosition.x = UnityEngine.Random.Range(currentPosition.x + minDistance, _randomMovingArea.width);
                        else
                            newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, currentPosition.x - minDistance);
                    }
                    else
                        newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, currentPosition.x - minDistance);
                }
                else
                    newPosition.x = UnityEngine.Random.Range(currentPosition.x + minDistance, _randomMovingArea.width);

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
                                newPosition.y = UnityEngine.Random.Range(currentPosition.y + minDistance, _randomMovingArea.height);
                            else
                                newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, currentPosition.y - minDistance);
                        }
                        else
                            newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, currentPosition.y - minDistance);
                    }
                    else
                        newPosition.y = UnityEngine.Random.Range(currentPosition.y + minDistance, _randomMovingArea.height);
                }
                else
                    newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, _randomMovingArea.height);
            }
            else
            {
                newPosition.x = UnityEngine.Random.Range(_randomMovingArea.x, _randomMovingArea.width);
                newPosition.y = UnityEngine.Random.Range(_randomMovingArea.y, _randomMovingArea.height);
            }

            MoveTo(newPosition, 1.5f);
        }
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
                Behaviours[PreviousBehaviourIndex].Stop();

            // TODO: Trigger signal to clear all bullets
            // TODO: Make sure we restore the initial boss state for transition

            if (Behaviours.Count > 0)
                Behaviours[CurrentBehaviourIndex].Start();
        }

        if (Behaviours.Count > 0)
            Behaviours[CurrentBehaviourIndex].Update();

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
        Behaviours[CurrentBehaviourIndex].TakeDamage(damage);

        OnTakeDamage.Invoke();
    }

    public float GetLifePercentage()
    {
        return Behaviours[CurrentBehaviourIndex].GetLifePercentage();
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
                var newPosition = Vector2.zero;
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
