using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LaserWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    [SerializeField] private bool bouncy = true;
    [SerializeField] private int damagePerShot = 5;
    [SerializeField] private GameObject emitter = default;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private float laserRenderTime = 0.3f;
    [SerializeField] private GameObject laserShotPrefab = default;
    [SerializeField] private float maxRange = 50.0f;
    [SerializeField] private LayerMask thingsWeHit = 0;

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
            var timeSinceLastShot = Time.time - _lastShotTime;
            if (timeSinceLastShot < fireRate)
                yield return new WaitForSeconds(timeSinceLastShot);

            ShootLaser(emitter.transform.position, emitter.transform.forward, maxRange);
            _lastShotTime = Time.time;
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void ShootLaser(Vector3 origin, Vector3 direction, float distanceToCast)
    {
        var hitCandidates = new List<(IDamageable damageable, float hitDistance)>();
        var ray = new Ray(origin, direction);
        var bounceMinHitDistance = float.PositiveInfinity;
        Vector3 bounceNormal = default;
        Vector3 bouncePoint = default;
        var hits = Physics.RaycastAll(ray, distanceToCast, thingsWeHit, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (bouncy && hit.collider.gameObject.layer == LayerMask.NameToLayer("LaserBouncer"))
                if (hit.distance < bounceMinHitDistance)
                {
                    bounceMinHitDistance = hit.distance;
                    bounceNormal = hit.normal;
                    bouncePoint = hit.point;
                }

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                hitCandidates.Add((damageable, hit.distance));
        }

        foreach (var (damageable, hitDistance) in hitCandidates)
            if (hitDistance < bounceMinHitDistance)
                damageable.TakeDamage(damagePerShot);

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
            var laserShotComponent = laser.GetComponent<LaserVisuals>();
            laserShotComponent.lifetime = laserRenderTime;
            lineRenderer.SetPosition(0, origin);

            var endPoint = origin + direction * hitDistance;
            lineRenderer.SetPosition(1, endPoint);
            laser.SetActive(true);
        }
    }
}