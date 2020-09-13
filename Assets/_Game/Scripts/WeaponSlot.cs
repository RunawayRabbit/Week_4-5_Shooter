
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponSlot : MonoBehaviour
{
    private GameObject WeaponObject;
    private Weapon currentWeapon;
    
    int PowerUpLayer;

    private bool _hasWeaponAttached = false;

    public void StartShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StopShooting();
    }

    private void Awake()
    {
        PowerUpLayer = LayerMask.NameToLayer("Powerup");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == PowerUpLayer &&
            other.gameObject.TryGetComponent<WeaponPowerUp>(out var powerUp))
        {
            if (_hasWeaponAttached)
                currentWeapon.Decomission();
           
            WeaponObject = Instantiate(powerUp.weaponPrefab, gameObject.transform);
            currentWeapon = WeaponObject.GetComponent<Weapon>();
            if (!currentWeapon)
                Debug.LogWarning($"{this.name} loaded a prefab from {other.gameObject.name}, but the prefab had no weapon component!");

            _hasWeaponAttached = true;
        }
    }
}
