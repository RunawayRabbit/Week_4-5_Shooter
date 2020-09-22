using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float _lastShotTime;
    public bool isShooting;

    public abstract void StartShooting();
    public abstract void StopShooting();

    private void OnDisable()
    {
        if (isShooting) StopShooting();
    }

    public virtual void Decomission()
    {
        Destroy(gameObject);
    }
}