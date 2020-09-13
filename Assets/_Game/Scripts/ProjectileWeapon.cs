using System.Collections;
using UnityEngine;

internal class ProjectileWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    
    protected readonly float _fireRate = 1.0f;
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
            Debug.Log("BRRRRRRR");
            yield return new WaitForSeconds(_fireRate); 
        }
    }
}