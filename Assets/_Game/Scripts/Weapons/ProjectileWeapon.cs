using System.Collections;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    [SerializeField] protected GameObject bulletPrefab = default;
    [SerializeField] protected float fireRate = 0.3f;
    [SerializeField] [Layer] protected int projectileLayer = default;

    public override void StartShooting()
    {
        if (isShooting) return;
        _shootingCoroutine = StartCoroutine(Shoot());
        isShooting = true;
    }

    public override void StopShooting()
    {
        if (!isShooting) return;
        StopCoroutine(_shootingCoroutine);
        isShooting = false;
    }

    public virtual IEnumerator Shoot()
    {
        while (true)
        {
            var timeSinceLastShot = Time.time - _lastShotTime;
            if (timeSinceLastShot < fireRate)
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
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}