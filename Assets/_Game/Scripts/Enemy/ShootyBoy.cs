using UnityEngine;

public class ShootyBoy : Enemy
{
    [SerializeField] private WeaponSlot weaponSlot = default;
    [SerializeField] private IEnemyMover mover;

    protected override void Awake()
    {
        mover = GetComponent<IEnemyMover>();
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
