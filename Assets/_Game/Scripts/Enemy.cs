using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHp = 10; 
    public int HP { get; private set; }

    private void Awake()
    {
        HP = startingHp;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("OWWIE!");
        HP -= damage;
        if (HP <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
