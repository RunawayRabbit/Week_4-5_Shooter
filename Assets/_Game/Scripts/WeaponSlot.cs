
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponSlot : MonoBehaviour
{
    private GameObject currentWeaponPrefab;
    int PowerUpLayer;

    private void Awake()
    {
        PowerUpLayer = LayerMask.NameToLayer("Powerup");
        Debug.Log($"Powerup layer set to {PowerUpLayer}");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == PowerUpLayer &&
            other.gameObject.TryGetComponent<WeaponPowerUp>(out var powerUp))
        {
            if (currentWeaponPrefab && currentWeaponPrefab.TryGetComponent<Weapon>(out var currentWeapon))
                currentWeapon.Decomission();
           
            currentWeaponPrefab = Instantiate(powerUp.weaponPrefab, gameObject.transform);
        }
    }
}
