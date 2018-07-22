using System;
using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    public float Speed = 1f;
    // Normalized position of bottom left and top right corners
    // that describe the bullet destruction area
    public Vector2 DestructionBoundsBottomLeft;
    public Vector2 DestructionBoundsTopRight;
    public float Damage = 1f;

    protected Vector2 Direction = Vector2.up;
    protected GameObject Emitter;

    private Rigidbody2D _rigidbody;
    private GameArea _gameArea;
    private Rect _gameBounds;
    private Vector2 _destructionBoundMin;
    private Vector2 _destructionBoundMax;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
            throw new Exception("No Rigidbody2D found for this bullet!");

        _gameArea = GameObject.FindGameObjectWithTag("GameArea").GetComponent<GameArea>();

        if (_gameArea == null)
            throw new Exception("No GameArea found in the scene!");

        _gameBounds = _gameArea.GetWorldRect();
    }

    public void Start()
    {
        var min = _gameBounds.min;
        var max = _gameBounds.max;
        var size = max - min;

        _destructionBoundMin = min + size * DestructionBoundsBottomLeft;
        _destructionBoundMax = max + -size * (Vector2.one - DestructionBoundsTopRight);
    }

    public void SetEmitter(GameObject emitter)
    {
        Emitter = emitter;
    }

    private void FixedUpdate()
    {
        var newPosition = transform.position;
        newPosition.x += Direction.x * Speed * Time.fixedDeltaTime;
        newPosition.y += Direction.y * Speed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(newPosition);

        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.x > _destructionBoundMax.x ||
            transform.position.x < _destructionBoundMin.x ||
            transform.position.y > _destructionBoundMax.y ||
            transform.position.y < _destructionBoundMin.y)
        {
            OnOutOfBounds();
        }
    }

    protected virtual void OnOutOfBounds()
    {
        Destroy(gameObject);
    }

    public Vector2 GetDirection()
    {
        return Direction;
    }

    /// <summary>
    /// Set bullet direction from angle in degrees
    /// </summary>
    /// <param name="angle">Angle value in degrees</param>
    public void SetDirectionFromAngle(float angle)
    {
        SetDirection(MathHelper.AngleToDirection(angle));
    }

    public void SetDirection(Vector2 value)
    {
        Direction = value;

        var rotation = MathHelper.DirectionToAngle(value);
        _rigidbody.MoveRotation(rotation);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
