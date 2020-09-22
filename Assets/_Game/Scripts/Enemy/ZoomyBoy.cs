using UnityEngine;

[SelectionBase]
public class ZoomyBoy : Enemy
{
    private float _previousCollision;
    [SerializeField] private float damageCooldown = 2.0f;
    [SerializeField] private int damagePerHit = 3;
    [SerializeField] private RammingMover rammingMover = default;

    protected override void Awake()
    {
        rammingMover = GetComponent<RammingMover>();
        base.Awake();
    }

    protected void Update()
    {
        //   ¯\_(ツ)_/¯
        rammingMover.Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") &&
            Time.time - _previousCollision > damageCooldown)
        {
            _previousCollision = Time.time;
            var damageable = other.GetComponent<IDamageable>();
            damageable.TakeDamage(damagePerHit);
        }
    }
}