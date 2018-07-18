using UnityEngine;

public class XmasSnowflakeBranch : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public float AngularSpeed;

    private bool _rotateClockwise;
    private float _rotationFactor = 1f;

    public void Start()
    {
        _rotateClockwise = Random.value > 0.5f;

        if (!_rotateClockwise)
            _rotationFactor = -1f;
    }

    public void FixedUpdate()
    {
        Rigidbody.MoveRotation(Rigidbody.rotation + (_rotationFactor * AngularSpeed * Time.fixedDeltaTime));
    }
}
