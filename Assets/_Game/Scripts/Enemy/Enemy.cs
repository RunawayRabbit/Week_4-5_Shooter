using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHp = 10;

    public int HP { get; private set; }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Die();
    }

    protected virtual void Awake()
    {
        HP = startingHp;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}