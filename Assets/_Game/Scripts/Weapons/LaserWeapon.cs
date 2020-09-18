using System.Collections;
using UnityEngine;

internal class LaserWeapon : Weapon
{
    [SerializeField] private GameObject emitter = default;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private float maxRange = 50.0f;
    [SerializeField] private int damagePerShot = 5;
    [SerializeField] private LayerMask thingsWeHit;

    private Coroutine _shootingCoroutine;

    public override void StartShooting()
    {
        _shootingCoroutine = StartCoroutine(Shoot());
    }
    
    public override void StopShooting()
    {
        StopCoroutine(_shootingCoroutine);
    }
    
    private IEnumerator Shoot()
    {
        while (true)
        {
            ShootLaser(emitter.transform.position, emitter.transform.forward, maxRange);
            yield return new WaitForSeconds(fireRate);   
        }
    }

    private void ShootLaser(Vector3 origin, Vector3 direction, float distanceToCast)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distanceToCast, thingsWeHit, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("LaserBouncer"))
            {
                Debug.Log($"We hit {hit.collider.name} Let's bounce!");
                ShootLaser(hit.point, Vector3.Reflect(direction, hit.normal), distanceToCast - hit.distance);
            }
                
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
                    
                damageable.TakeDamage(damagePerShot);

        }
    }
}