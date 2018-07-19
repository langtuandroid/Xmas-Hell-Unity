using System.Collections;
using UnityEngine;

public class XmasSnowflakeBranch : AbstractEntity
{
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

        var bossGameObject = GameObject.FindGameObjectWithTag("Enemy");

        if (bossGameObject != null)
            _boss = bossGameObject.GetComponent<AbstractBoss>();
        else
            Debug.LogWarning("No boss found in this scene");

        _timeBeforeRush = Random.Range(0, 2);

        StartCoroutine(RushOnPlayer());
    }

    public void SetBoss(AbstractBoss boss)
    {
        _boss = boss;
    }

    public IEnumerator RushOnPlayer()
    {
        yield return new WaitForSeconds(_timeBeforeRush);

        var angle = MathHelper.DirectionToAngle(_boss.Player.transform.position - transform.position) + 180f;
        Rigidbody.MoveRotation(angle);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -MathHelper.AngleToDirection(angle), 2300, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            MoveTo(hit.point);
            _rushing = true;
        }
        else
        {
            Debug.LogWarning("No wall found by this Xmas Snowflake branch!");
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_rushing)
            Rigidbody.MoveRotation(Rigidbody.rotation + (_rotationFactor * AngularVelocity * Time.fixedDeltaTime));
    }
}
