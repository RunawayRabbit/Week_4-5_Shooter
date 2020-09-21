using System.Collections;
using UnityEngine;

internal class ProjectileWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    [SerializeField] protected float fireRate = 0.3f;
    [SerializeField, Layer] protected int projectileLayer = default;
    [SerializeField] private GameObject bulletPrefab = default;

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
            // Fire a bullet!
            var bullet =  PoolManager.Instance.Get(bulletPrefab.name);
            if (bullet)
            {
                var trans = transform;
                bullet.transform.position = trans.position;
                bullet.transform.rotation = trans.rotation;
                if(projectileLayer != 0) bullet.gameObject.layer = projectileLayer;
                bullet.SetActive(true);
            }
            
            yield return new WaitForSeconds(fireRate); 
        }
    }
}