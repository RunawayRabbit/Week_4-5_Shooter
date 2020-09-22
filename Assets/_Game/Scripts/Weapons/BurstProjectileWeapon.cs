using System.Collections;
using UnityEngine;

public class BurstProjectileWeapon : ProjectileWeapon
{
    private int _shotsFired = 0;
    [SerializeField] private float burstCooldown = 2.0f;
    [SerializeField] private int shotsPerBurst = 3;

    public override IEnumerator Shoot()
    {
        while (true)
        {
            var currentFireRate = _shotsFired % shotsPerBurst == 0 ? burstCooldown : fireRate;
            var timeSinceLastShot = Time.time - _lastShotTime;
            if (timeSinceLastShot < currentFireRate)
                yield return new WaitForSeconds(timeSinceLastShot);

            // Fire a bullet!
            var bullet = PoolManager.Instance.Get(bulletPrefab.name);
            if (bullet)
            {
                var trans = transform;
                bullet.transform.position = trans.position;
                bullet.transform.rotation = trans.rotation;
                if (projectileLayer != 0) bullet.gameObject.layer = projectileLayer;
                bullet.SetActive(true);
                _shotsFired++;
            }

            yield return new WaitForSeconds(currentFireRate);
        }
    }
}