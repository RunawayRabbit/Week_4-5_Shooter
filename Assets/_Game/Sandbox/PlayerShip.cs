
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] GameObject target = default;
    [SerializeField] private float _distanceFromTarget = 3.0f;
    [SerializeField] private float _moveSpeed = 4.0f;
    [SerializeField] private float overShoulderLag = 0.4f;
    [SerializeField] private float weaponRotateThresholdVelocity = 0.333f;
    
    private Arena _arena = default;
    private Vector3 _velocity = default;

    private bool _locked;

    public WeaponSlot[] weaponSlots;

    private void Awake()
    {
        if(!target) Debug.Assert(target, $"{gameObject.name} does not have a target object set!");
        
        _arena = GetComponentInParent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} has no Arena in it's parent..");
    }

    private void Update()
    {
        var targetTransform = target.transform;
        var currentTransform = transform;

        Quaternion look = Quaternion.LookRotation(
            targetTransform.localPosition - currentTransform.localPosition, Vector3.up);

        var desiredPosition = target.transform.localPosition + Vector3.back * _distanceFromTarget;

        float smoothTime = _arena.CurrentMode == Arena.Mode.Horizontal ? 0.01f : overShoulderLag;
        
        Vector3 newPosition = Vector3.SmoothDamp(currentTransform.position, desiredPosition, ref _velocity, smoothTime, _moveSpeed);

        currentTransform.position = newPosition;
        currentTransform.localRotation = look;
    }

    // ReSharper disable once UnusedMember.Global
    public void Move(InputAction.CallbackContext context)
    {
        //@NOTE: Actual movement isn't handled here! It's handled in the target reticle, we just follow that around.
        // This callback is only for managing things that also happen when we move.
        
        // Rotate weapons
        var input = context.ReadValue<Vector2>();
        var input3D =  input.sqrMagnitude < weaponRotateThresholdVelocity ?
            Vector3.zero :
            new Vector3(input.x, 0.0f, input.y);

        if (_locked) return;
        foreach (var weaponSlot in weaponSlots)
            weaponSlot.Rotate(input3D);
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
                break;
            case InputActionPhase.Canceled:
                _locked = false;
                break;
        }
    }
}
