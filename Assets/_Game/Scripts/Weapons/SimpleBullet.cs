
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] float lifetime = 10.0f;   
    [SerializeField] int damage = 2;

    private Coroutine _lifetimeTracker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        
        _lifetimeTracker = StartCoroutine(DeathClock());
    }

    private void OnDisable()
    {
        StopCoroutine(_lifetimeTracker);
    }

    public IEnumerator DeathClock()
    {
        yield return new WaitForSeconds(lifetime);
        PoolManager.Instance.Return(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(damage);
        
        PoolManager.Instance.Return(gameObject);
    }
}
