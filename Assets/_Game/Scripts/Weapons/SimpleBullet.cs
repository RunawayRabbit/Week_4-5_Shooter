
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] float lifetime = 10.0f;

    private Coroutine lifetimeTracker;
    public Pool pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        
        lifetimeTracker = StartCoroutine(DeathClock());
    }

    private void OnDisable()
    {
        StopCoroutine(lifetimeTracker);
    }

    public IEnumerator DeathClock()
    {
        yield return new WaitForSeconds(lifetime);
        pool.Return(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        //COLLIDE
        //pool.Return(gameObject);
    }
}
