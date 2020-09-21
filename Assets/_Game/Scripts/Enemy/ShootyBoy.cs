using UnityEngine;

public class ShootyBoy : Enemy
{
    [SerializeField] private WeaponSlot weaponSlot = default;

    private void OnEnable()
    {
        weaponSlot.StartShooting();
    }
}
