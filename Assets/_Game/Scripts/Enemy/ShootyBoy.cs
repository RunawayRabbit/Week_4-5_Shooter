using UnityEngine;

public class ShootyBoy : Enemy
{
    [SerializeField] private WeaponSlot weaponSlot = default;
    [SerializeField] private IMover mover;

    protected override void Awake()
    {
        mover = GetComponent<IMover>();
        base.Awake();
    }
    
    private void Update()
    {
        mover?.Move();
    }

    private void OnEnable()
    {
        weaponSlot.StartShooting();
    }
}
