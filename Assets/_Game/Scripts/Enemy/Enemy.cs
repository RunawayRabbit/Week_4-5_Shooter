using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHp = 10;
    public int HP { get; private set; }

    protected virtual void Awake()
    {
        HP = startingHp;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log($"{name} took {damage} damage, {HP} remaining.");
        if (HP <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
