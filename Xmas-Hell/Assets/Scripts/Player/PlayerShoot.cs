using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Bullet;
    public float FireRate = 0.05f;
    public float BulletSpeed = 10;
    public List<Transform> ShootingPoints;
    public AudioSource ShootSound;

    private float _nextFire = 0f;

    void Update()
    {
        if (ShootingPoints.Count == 0)
            return;

        if (Input.GetMouseButton(0) && Time.time > _nextFire)
        {
            _nextFire = Time.time + FireRate;

            foreach (var shootingPoint in ShootingPoints)
            {
                // TODO: Use a pool
                var playerBulletObject = Instantiate(Bullet);
                var bulletScript = playerBulletObject.GetComponent<AbstractBullet>();

                bulletScript.Speed = BulletSpeed;
                bulletScript.SetEmitter(gameObject);

                playerBulletObject.transform.position = shootingPoint.transform.position;

                // TODO: Should be computed only once in the Start method
                var shootingPointRotation = shootingPoint.transform.localRotation.eulerAngles.z;
                bulletScript.SetDirectionFromAngle(shootingPointRotation);
            }

            if (ShootSound)
                AudioSource.PlayClipAtPoint(ShootSound.clip, transform.position);
        }
    }
}
