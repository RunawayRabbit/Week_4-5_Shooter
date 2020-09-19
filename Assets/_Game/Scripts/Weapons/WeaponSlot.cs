using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    //@TODO: PRETTIFY THE INSPECTOR WITH HEADERS, RANGES AND SO ON
    protected GameObject WeaponObject;
    protected Weapon CurrentWeapon;

    protected bool HasWeaponAttached = false;
    [HideInInspector] public Vector3 minRotation = Vector3.forward;
    [HideInInspector] public Vector3 maxRotation = Vector3.forward;

    [SerializeField] private GameObject startingWeapon = default;
    
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

    protected void EquipWeapon(GameObject weaponPrefab)
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