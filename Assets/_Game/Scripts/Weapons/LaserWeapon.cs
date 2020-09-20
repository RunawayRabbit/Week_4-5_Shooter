using System;
using System.Collections;
using UnityEngine;

internal class LaserWeapon : Weapon
{
    [SerializeField] private GameObject emitter = default;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private float maxRange = 50.0f;
    [SerializeField] private int damagePerShot = 5;
    [SerializeField] private float laserRenderTime = 0.3f;
    [SerializeField] private LayerMask thingsWeHit = 0;
    [SerializeField] private bool bouncy = true;
    [SerializeField] private GameObject laserShotPrefab = default;
    
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
        float bounceMinHitDistance = Single.PositiveInfinity;
        Vector3 bounceNormal = default;
        Vector3 bouncePoint = default;
        RaycastHit[] hits = Physics.RaycastAll(ray, distanceToCast, thingsWeHit, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (bouncy && hit.collider.gameObject.layer == LayerMask.NameToLayer("LaserBouncer"))
            {
                if(hit.distance < bounceMinHitDistance)
                {
                    bounceMinHitDistance = hit.distance;
                    bounceNormal = hit.normal;
                    bouncePoint = hit.point;
                }
            }
                
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(damagePerShot);
        }

        if (bounceMinHitDistance < distanceToCast)
        {
            DrawLaser(origin, direction, bounceMinHitDistance);
            ShootLaser(bouncePoint, Vector3.Reflect(direction, bounceNormal), distanceToCast - bounceMinHitDistance);
        }
        else
        {
            DrawLaser(origin, direction, distanceToCast);            
        }
    }

    private void DrawLaser(Vector3 origin, Vector3 direction, float hitDistance)
    {
        var laser = PoolManager.Instance.Get(laserShotPrefab.name);
        if (laser)
        {
            var lineRenderer = laser.GetComponent<LineRenderer>();
            var laserShotComponent = laser.GetComponent<LaserShot>();
            laserShotComponent.lifetime = laserRenderTime;
            lineRenderer.SetPosition(0, origin);

            var endPoint = origin + (direction * hitDistance);
            lineRenderer.SetPosition(1, endPoint);
            laser.SetActive(true);     
        }
    }
}