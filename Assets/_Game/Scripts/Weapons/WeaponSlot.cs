using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    protected Weapon CurrentWeapon = default;

    protected bool HasWeaponAttached = false;

    //@TODO: PRETTIFY THE INSPECTOR WITH HEADERS, RANGES AND SO ON
    [SerializeField] private GameObject startingWeapon = default;

    protected GameObject WeaponObject = default;

    public void StartShooting()
    {
        if (HasWeaponAttached) CurrentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (HasWeaponAttached) CurrentWeapon.StopShooting();
    }

    protected virtual void Awake()
    {
        if (startingWeapon)
            EquipWeapon(startingWeapon);
    }

    public virtual void EquipWeapon(GameObject weaponPrefab)
    {
        if (HasWeaponAttached)
            CurrentWeapon.Decomission();

        WeaponObject = Instantiate(weaponPrefab, gameObject.transform);
        WeaponObject.layer = gameObject.layer;
        CurrentWeapon = WeaponObject.GetComponent<Weapon>();
        if (!CurrentWeapon)
            Debug.LogWarning($"{name} loaded a prefab {gameObject.name}, but the prefab had no weapon component!");

        HasWeaponAttached = true;
    }
}