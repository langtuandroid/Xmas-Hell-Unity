using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    public float Speed = 1f;
    public Bounds DestructionBounds;
    public float Damage = 1f;

    protected Vector2 Direction = Vector2.up;
    protected GameObject Emitter;

    public void SetEmitter(GameObject emitter)
    {
        Emitter = emitter;
    }

    private void FixedUpdate()
    {
        var newPosition = transform.position;
        newPosition.x += Direction.x * Speed * Time.deltaTime;
        newPosition.y += Direction.y * Speed * Time.deltaTime;

        transform.position = newPosition;

        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.x > DestructionBounds.extents.x ||
            transform.position.x < -DestructionBounds.extents.x ||
            transform.position.y > DestructionBounds.extents.y ||
            transform.position.y < -DestructionBounds.extents.y)
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
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
