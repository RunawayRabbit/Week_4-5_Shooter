using System.Collections;
using UnityEngine;

internal class LaserWeapon : Weapon
{
    [SerializeField] GameObject Emitter;
    [SerializeField] float fireRate = 0.8f;
    
    
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
            
            yield return new WaitForSeconds(fireRate);   
        }
    }
}