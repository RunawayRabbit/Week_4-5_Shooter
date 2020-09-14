using System;
using UnityEngine;

[RequireComponent(typeof(Collider)), SelectionBase]
public class WeaponSlot : MonoBehaviour
{
    private GameObject WeaponObject;
    private Weapon currentWeapon;
    private SphereCollider ourCollider;

    [SerializeField] private float colliderRadiusWhileEmpty = 2.0f;
    [SerializeField] private float colliderRadiusWhileActive = 1.0f;
    
    [SerializeField] private float rotateSpeed = 50.0f;
    
    private int _powerUpLayer;

    private bool _hasWeaponAttached = false;
    [SerializeField] private Vector3 minRotation = Vector3.forward;
    [SerializeField] public Vector3 maxRotation;
    [SerializeField] public float turningArc;

    private Vector3 _targetVector = Vector3.forward;
    private GameObject _targetReticule = default;
    private float _arcWindingDirection = default;

    private Arena.Mode _currentMode = Arena.Mode.Horizontal;

    public void StartShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StopShooting();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);
    }
    #endif
    
    private void Awake()
    {
        _powerUpLayer = LayerMask.NameToLayer("Powerup");
        
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);
        
        _targetReticule = GameObject.FindWithTag("TargetReticule");
        if(!_targetReticule) Debug.LogWarning("Target Reticle not found! Remember to set the TAG for it!");

        ourCollider = GetComponent<SphereCollider>();
        if(!ourCollider) Debug.LogWarning($"{this.name} has no SphereCollider on it. We can't pick up powerups without one!");
    }

    private void OnEnable() => Arena.Instance.OnModeChange += ChangeMode;
    private void OnDisable() => Arena.Instance.OnModeChange -= ChangeMode;
    
    private void ChangeMode(Arena.Mode newMode) => _currentMode = newMode;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == _powerUpLayer &&
            other.gameObject.TryGetComponent<WeaponPowerUp>(out var powerUp))
        {
            if (_hasWeaponAttached)
                currentWeapon.Decomission();
           
            WeaponObject = Instantiate(powerUp.weaponPrefab, gameObject.transform);
            currentWeapon = WeaponObject.GetComponent<Weapon>();
            if (!currentWeapon)
                Debug.LogWarning($"{this.name} loaded a prefab from {other.gameObject.name}, but the prefab had no weapon component!");

            _hasWeaponAttached = true;
            ourCollider.radius = colliderRadiusWhileActive;
        }
    }

    private void Update()
    {
        Vector3 pointAt;
        if (_currentMode == Arena.Mode.Horizontal)
            pointAt = _targetVector;
        else
            pointAt = _targetReticule.transform.position - transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(pointAt, Vector3.up),
            rotateSpeed * Time.deltaTime);  
    }

    public void Rotate(Vector3 input)
    {
        if (_currentMode == Arena.Mode.Horizontal)
        {
            if (rotateSpeed <= 0.0f) return;
            _targetVector = input == Vector3.zero ? transform.forward : SelectFacingVector(input);
        }
    }

    private Vector3 SelectFacingVector(Vector3 input)
    {
        float minToInputWinding = Vector3.Dot(Vector3.Cross(minRotation, input), Vector3.up);
        float maxToInputWinding = Vector3.Dot(Vector3.Cross(maxRotation, input), Vector3.up);

        bool isMinToInputSameWinding = Mathf.Sign(_arcWindingDirection) == Math.Sign(minToInputWinding);
        bool isMaxToInputSameWinding = Mathf.Sign(_arcWindingDirection) == Math.Sign(maxToInputWinding);

        if (!isMinToInputSameWinding && !isMaxToInputSameWinding) return maxRotation;
        return minRotation;
    }
}
