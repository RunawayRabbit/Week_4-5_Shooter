using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Beam,
        Missile
    }

    protected bool _isShooting;
    [SerializeField] private InputActionReference shootReference = default;
    
    public abstract void StartShooting();
    public abstract void StopShooting();
    
    private void Awake()
    {
        Debug.Assert(shootReference, $"{name} has no shootReference set. Make sure you have selected the \"shoot\" action input in the inspector.");
        // Start shooting immediately if the button is down.
        if (shootReference.action.phase == InputActionPhase.Started)
            StartShooting();
    }
    
    private void OnDisable()
    {
        if (_isShooting) StopShooting();
    }
    
    public void Decomission()
    {
        Debug.Log("Weapon is being decomissioned");
        Destroy(gameObject);
    }
    
}
