
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Beam,
        Missile
    }

    protected bool _isShooting;
    protected float _fireRate;
    
    public abstract void StartShooting();
    public abstract void StopShooting();
    public void Decomission()
    {
        Debug.Log("Weapon is being decomissioned");
        Destroy(gameObject);
    }
}
