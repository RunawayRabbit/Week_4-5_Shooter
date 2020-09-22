
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject target = default;
    [SerializeField] private float _distanceFromTarget = 3.0f;
    [SerializeField] private float _moveSpeed = 4.0f;
    [SerializeField] private float overShoulderLag = 0.4f;
    [SerializeField] private float weaponRotateThresholdVelocity = 0.333f;

    [SerializeField] private int maxHp = 30;
    public int HP { get; private set; }
    
    private Arena _arena;
    private Vector3 _velocity = default;

    private bool _locked;

    public PlayerWeaponSlot[] weaponSlots;
    private Vector3 _input3D;

    [SerializeField] private float shieldUptime;
    [SerializeField] private float shieldCooldown;
    private bool _isShielded = false;
    private bool _isShieldOnCooldown = false;

    private void Start()
    {
        HP = maxHp;
        _arena = Arena.Instance;
    }

    private void OnDisable() => StopAllCoroutines();


    private void Update()
    {
        var targetTransform = target.transform;
        var currentTransform = transform;

        Quaternion look = Quaternion.LookRotation(
            targetTransform.localPosition - currentTransform.localPosition, Vector3.up);

        var desiredPosition = target.transform.localPosition + Vector3.back * _distanceFromTarget;

        float smoothTime = _arena.CurrentMode == Arena.Mode.Horizontal ? 0.01f : overShoulderLag;
        
        Vector3 newPosition = Vector3.SmoothDamp(currentTransform.localPosition, desiredPosition, ref _velocity, smoothTime, _moveSpeed);

        currentTransform.localPosition = newPosition;
        currentTransform.localRotation = look;
    }

    // ReSharper disable once UnusedMember.Global
    public void Move(InputAction.CallbackContext context)
    {
        //@NOTE: Movement from input isn't handled here! It's handled in the target
        // reticule, we just follow that around.
        // This callback is only for managing things that also happen when we move.
        
        // Rotate weapons
        var input = context.ReadValue<Vector2>();
        _input3D = input.sqrMagnitude < weaponRotateThresholdVelocity ?
            Vector3.zero :
            new Vector3(input.x, 0.0f, input.y);
        
        if (_locked) return;
        foreach (var weaponSlot in weaponSlots)
            weaponSlot.Rotate(_input3D);
    }

    // ReSharper disable once UnusedMember.Global
    public void Shoot(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
            foreach (var weaponSlot in weaponSlots)
            {
                weaponSlot.StartShooting();
            }
            break;
            case InputActionPhase.Canceled:
            foreach (var weaponSlot in weaponSlots)
            {
                weaponSlot.StopShooting();
            }
            break;
        }
    }
    
    // ReSharper disable once UnusedMember.Global
    public void Lock(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _locked = true;
                foreach (var weaponSlot in weaponSlots)
                    weaponSlot.StopRotation();
                break;
            
            case InputActionPhase.Canceled:
                _locked = false;
                foreach (var weaponSlot in weaponSlots)
                    weaponSlot.Rotate(_input3D);
                break;
        }
    }
    
    // ReSharper disable once UnusedMember.Global
    public void Shield(InputAction.CallbackContext context)
    {
        if(!_isShieldOnCooldown)
        {
            Debug.Log("Shields go up");
             if (context.phase == InputActionPhase.Started)
             {
                 _isShielded = true;
                 _isShieldOnCooldown = true;
                 StartCoroutine(RaiseShields());
                 StartCoroutine(ShieldCooldown());
             }
        }
    }

    private IEnumerator ShieldCooldown()
    {
        yield return new WaitForSeconds(shieldCooldown);
        Debug.Log("Shields ready to go!");
        _isShieldOnCooldown = false;
    }

    private IEnumerator RaiseShields()
    {
        yield return new WaitForSeconds(shieldUptime);
        Debug.Log("Shields go down");
        _isShielded = false;
    }

    public void TakeDamage(int damage)
    {
        if(!_isShielded)
            {
            Debug.Log($"Player HP left: {HP}");
            HP -= damage;
            if (HP <= 0) Die();
        }
        else
        {
            Debug.Log($"Hit mitigated!");
        }
    }

    private void Die()
    {
        Debug.Log("Oops we died.");
        //do nothing
    }
}
