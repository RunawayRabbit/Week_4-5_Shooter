using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHp = 10;
    [CanBeNull, SerializeField] private IMover mover;

    public int HP { get; private set; }

    protected virtual void Awake()
    {
        HP = startingHp;
        mover = GetComponent<IMover>();
    }
    
    protected virtual void Update()
    {
        mover?.Move();
    }
    
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
