using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    //@TODO: PRETTIFY THE INSPECTOR WITH HEADERS, RANGES AND SO ON
    [SerializeField] private GameObject startingWeapon = default;

    protected GameObject WeaponObject = default;
    protected Weapon CurrentWeapon = default;

    protected bool HasWeaponAttached = false;

    
    public void StartShooting()
    {
        if (HasWeaponAttached) CurrentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (HasWeaponAttached) CurrentWeapon.StopShooting();
    }
    
    protected void Awake()
    {
        if(startingWeapon)
            EquipWeapon(startingWeapon);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (HasWeaponAttached)
            CurrentWeapon.Decomission();

        WeaponObject = Instantiate(weaponPrefab, gameObject.transform);
        CurrentWeapon = WeaponObject.GetComponent<Weapon>();
        if (!CurrentWeapon)
            Debug.LogWarning($"{this.name} loaded a prefab {gameObject.name}, but the prefab had no weapon component!");

        HasWeaponAttached = true;
    }
}