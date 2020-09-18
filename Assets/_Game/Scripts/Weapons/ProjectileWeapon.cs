using System.Collections;
using UnityEngine;

internal class ProjectileWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    private Pool _bulletPool;
    
    [SerializeField] protected float fireRate = 0.3f;
    private GameObject _bulletPrefab;

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
            var bullet =  Pool.Instance.Get();
            if (bullet)
            {
                var trans = transform;
                bullet.transform.position = trans.position;
                bullet.transform.rotation = trans.rotation;
                bullet.SetActive(true);
            }
            
            yield return new WaitForSeconds(fireRate); 
        }
    }
}