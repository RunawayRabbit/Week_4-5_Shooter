using System.Collections;
using UnityEngine;

internal class BulletWeapon : Weapon
{
    public override void StartShooting()
    {
        Debug.Log("Bullet weapon goes brrr");
        StartCoroutine(Shoot());
    }

    public override void StopShooting()
    {
        Debug.Log("Bullet weapon stops going brrr");
        StopCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        //Do the thing to fire the bullet.
        Debug.Log("Fire The Bullet!");
        yield return new WaitForSeconds(_fireRate);
    }
}