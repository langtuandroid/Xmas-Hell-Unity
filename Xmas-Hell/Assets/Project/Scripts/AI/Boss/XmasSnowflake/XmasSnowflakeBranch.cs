﻿using System.Collections;
using UnityEngine;

public class XmasSnowflakeBranch : AbstractEntity
{
    #region Serialize fields

    [SerializeField] private Transform _target = null;
    [SerializeField] private float _accelerationOverTime = 0.01f;
    [SerializeField] private float _maxAcceleration = 5f;
    [SerializeField] private float _rushTimeMin = 1f;
    [SerializeField] private float _rushTimeMax = 10f;
    [SerializeField] private string _patternName = "default";

    #endregion

    private bool _rotateClockwise;
    private float _rotationFactor = 1f;
    private AbstractBoss _boss;
    private float _timeBeforeRush;
    private bool _rushing;

    protected override void Start()
    {
        base.Start();

        _rotateClockwise = Random.value > 0.5f;
        _rushing = false;

        if (!_rotateClockwise)
            _rotationFactor = -1f;

        _timeBeforeRush = Random.Range(_rushTimeMin, _rushTimeMax);

        StartCoroutine(RushOnTarget());
    }

    public void Initialize(AbstractBoss boss)
    {
        _boss = boss;
    }

    public void OnDestroy()
    {
        StopCoroutine(RushOnTarget());
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public IEnumerator RushOnTarget()
    {
        yield return new WaitForSeconds(_timeBeforeRush);

        var direction = _target.transform.position - transform.position;
        var angle = MathHelper.DirectionToAngle(direction);
        Rotation = angle;
        Direction = -MathHelper.AngleToDirection(angle);
        _rushing = true;

        while (Acceleration.x < _maxAcceleration && Acceleration.y < _maxAcceleration)
        {
            Acceleration += Vector2.one * _accelerationOverTime;
            yield return null;
        }
    }

    public override void TakeDamage(float damage)
    {
        _boss.TakeDamage(damage / 8f);
    }

    protected override void FixedUpdate()
    {
        if (_pause)
            return;

        base.FixedUpdate();

        if (!_rushing)
            Rigidbody.MoveRotation(Rigidbody.rotation + (_rotationFactor * _angularVelocity * Time.fixedDeltaTime));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsAlive = false;
            _boss.ShootPattern(_patternName, transform.position);
        }
    }
}