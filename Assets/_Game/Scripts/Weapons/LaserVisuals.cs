using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVisuals : MonoBehaviour
{
    public float lifetime = 1.0f;
    [SerializeField] public Color startColor;
    [SerializeField] public Color endColor;
    
    private float birth;
    private Coroutine _lifetimeTracker;
    private LineRenderer _lineRenderer;
    

    private void OnEnable()
    {
        birth = Time.time;
        if (!_lineRenderer) _lineRenderer = GetComponent<LineRenderer>();
        _lifetimeTracker = StartCoroutine(DeathClock());
    }

    private void OnDisable()
    {
        StopCoroutine(_lifetimeTracker);
    }

    private void Update()
    {
        float t = (Time.time - birth) / lifetime;
        _lineRenderer.startColor = Color.Lerp(startColor, endColor, Mathf.Min(3.0f*t, 1.0f));
        _lineRenderer.endColor = Color.Lerp(startColor, endColor, t);
    }

    public IEnumerator DeathClock()
    {
        yield return new WaitForSeconds(lifetime);
        PoolManager.Instance.Return(gameObject);
    }
}
