using UnityEngine;

public class PlayerBullet : AbstractBullet
{
    protected override void CheckOutOfBounds()
    {
        if (transform.position.x > DestructionBounds.extents.x ||
            transform.position.x < -DestructionBounds.extents.x ||
            transform.position.y > DestructionBounds.extents.y ||
            transform.position.y < -DestructionBounds.extents.y)
        {
            HideBullet();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        HideBullet();
    }

    // This GameObject will be retrieved from a pool, we don't want to destroy it
    private void HideBullet()
    {
        gameObject.SetActive(false);
    }
}
