using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Bullet;
    public float FireRate = 0.05f;
    public float BulletSpeed = 10;
    public List<Transform> ShootingPoints;
    public AudioSource ShootSound;
    public int BulletsPoolSize;

    private float _nextFire = 0f;
    private Queue<GameObject> _bulletsPool;

    private void Start()
    {
        _bulletsPool = new Queue<GameObject>();

        for (int i = 0; i < BulletsPoolSize; i++)
        {
            var bullet = Instantiate(Bullet);
            bullet.SetActive(false);
            _bulletsPool.Enqueue(bullet);
        }
    }

    void Update()
    {
        if (ShootingPoints.Count == 0)
            return;

        if (Input.GetMouseButton(0) && Time.time > _nextFire)
        {
            _nextFire = Time.time + FireRate;

            foreach (var shootingPoint in ShootingPoints)
            {
                var playerBulletObject = _bulletsPool.Dequeue();
                playerBulletObject.SetActive(true);
                var bulletScript = playerBulletObject.GetComponent<AbstractBullet>();

                bulletScript.Speed = BulletSpeed;
                bulletScript.SetEmitter(gameObject);

                playerBulletObject.transform.position = shootingPoint.transform.position;

                // TODO: Should be computed only once in the Start method
                var shootingPointRotation = shootingPoint.transform.localRotation.eulerAngles.z;
                bulletScript.SetDirectionFromAngle(shootingPointRotation);

                _bulletsPool.Enqueue(playerBulletObject);
            }

            if (ShootSound)
                AudioSource.PlayClipAtPoint(ShootSound.clip, transform.position);
        }
    }
}
