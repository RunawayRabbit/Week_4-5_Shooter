using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    [SerializeField] public float lifetime = 1.0f;

    private Coroutine _lifetimeTracker;

    private void OnEnable()
    {
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
}
