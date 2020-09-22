using UnityEditor;
using UnityEngine;

public class RotatingWeaponSlot : WeaponSlot
{
    [SerializeField] private float maxTurnSpeed = 2.0f;
    [SerializeField] private float minCosineToShoot = 0.86f;
    [SerializeField] private Transform player = default;
    [SerializeField] private float shootRadius = 20.0f;

    protected override void Awake()
    {
        if (!player)
            player = GameObject.FindWithTag("Player").transform;

        base.Awake();
        StopIfShooting();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1.0f, 0.2f, 0.2f, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, shootRadius);
    }

    private void Update()
    {
        var shootRadiusSq = shootRadius * shootRadius;
        var facingVectorToTarget = player.transform.position - transform.position;

        var lookAtTarget = Quaternion.LookRotation(facingVectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtTarget, maxTurnSpeed);

        var distanceToTargetSq = facingVectorToTarget.sqrMagnitude;
        if (distanceToTargetSq < shootRadiusSq)
        {
            var cosine = Vector3.Dot(transform.forward.normalized, facingVectorToTarget.normalized);
            if (cosine > minCosineToShoot)
                StartShooting();
            else StopShooting();
        }
        else
        {
            StopShooting();
        }
    }

    private void StopIfShooting()
    {
        if (HasWeaponAttached && CurrentWeapon.isShooting) CurrentWeapon.StopShooting();
    }
}