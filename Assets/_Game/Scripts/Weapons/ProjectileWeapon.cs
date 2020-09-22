using System.Collections;
using UnityEngine;

internal class ProjectileWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] protected float fireRate = 0.3f;
    [SerializeField] [Layer] protected int projectileLayer = default;

    public override void StartShooting()
    {
        _shootingCoroutine = StartCoroutine(Shoot());
    }

    public override void StopShooting()
    {
        StopCoroutine(_shootingCoroutine);
    }

    public IEnumerator Shoot()
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