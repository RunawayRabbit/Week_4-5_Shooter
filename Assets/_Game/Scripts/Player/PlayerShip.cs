
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

    public int HP { get; private set; }
    [SerializeField] private int maxHp = 30;
    
    private Arena _arena;
    private Vector3 _velocity = default;

    private Vector3 _input3D;

    public PlayerWeaponSlot[] weaponSlots;
    private bool _locked;

    private PlayerShields _shields;

    private void Awake() //@BUG: formally Start. Can we make this Awake now?
    {
        HP = maxHp;
        _arena = Arena.Instance;
        _shields = GetComponent<PlayerShields>();
    }

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
    public void Shield(InputAction.CallbackContext context) => _shields.Shield(context);

    public void TakeDamage(int damage)
    {
        if(!_shields.AreShieldsUp)
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
