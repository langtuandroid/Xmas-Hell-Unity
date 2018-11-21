using UnityEngine;

public class XmasBallBehaviour4 : AbstractBossBehaviour
{
    [SerializeField] private Vector2 _maxAcceleration;

    private Vector2 _initialAcceleration = Vector2.zero;

    public override void Initialize(AbstractBoss boss)
    {
        base.Initialize(boss);

        Boss.AngularVelocity = 300f;
        _initialAcceleration = Boss.Acceleration;
    }

    public override void Step()
    {
        base.Step();

        Boss.Acceleration = _initialAcceleration + (1f - GetLifePercentage()) * _maxAcceleration;
    }
}