using UnityEngine;

public class ShootyBoy : Enemy
{
    [SerializeField] private WeaponSlot weaponSlot;

    private void OnEnable()
    {
        weaponSlot.StartShooting();
    }
}
