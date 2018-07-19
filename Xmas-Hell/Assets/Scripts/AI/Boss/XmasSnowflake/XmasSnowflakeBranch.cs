using System.Collections;
using UnityEngine;

public class XmasSnowflakeBranch : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public float AngularSpeed;

    private bool _rotateClockwise;
    private float _rotationFactor = 1f;
    private AbstractBoss _boss;
    private float _timeBeforeRush;
    private bool _rushing;

    public void Start()
    {
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

        Rigidbody.AddForce(transform.right * 1);
        _rushing = true;
    }

    public void FixedUpdate()
    {
        if (!_rushing)
            Rigidbody.MoveRotation(Rigidbody.rotation + (_rotationFactor * AngularSpeed * Time.fixedDeltaTime));
    }
}
