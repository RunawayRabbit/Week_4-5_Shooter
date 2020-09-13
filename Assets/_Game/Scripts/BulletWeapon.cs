using System.Collections;
using UnityEngine;

internal class BulletWeapon : Weapon
{
    private Coroutine _shootingCoroutine;
    
    protected readonly float _fireRate = 1.0f;
    public override void StartShooting()
    {
        Debug.Log("START SHOOTING");
        _shootingCoroutine = StartCoroutine(Shoot());
    }

    public override void StopShooting()
    {
        Debug.Log("STOP SHOOTING");
        StopCoroutine(_shootingCoroutine);
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            Debug.Log("Fire The Bullet!");
            yield return new WaitForSeconds(_fireRate); 
        }
    }
}