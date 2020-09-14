
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponSlot : MonoBehaviour
{
    private GameObject WeaponObject;
    private Weapon currentWeapon;
    
    [SerializeField] private float rotateSpeed = 50.0f;
    
    int PowerUpLayer;

    private bool _hasWeaponAttached = false;
    
    public float turningArc;
    
    [SerializeField] private Vector3 minRotation = Vector3.forward;
    [SerializeField] public Vector3 maxRotation;
    
    private Vector3 _targetVector = Vector3.forward;
    private float _arcWindingDirection = default;

    public void StartShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StopShooting();
    }

    private void OnValidate()
    {
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);
    }
    
    private void Awake()
    {
        PowerUpLayer = LayerMask.NameToLayer("Powerup");
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == PowerUpLayer &&
            other.gameObject.TryGetComponent<WeaponPowerUp>(out var powerUp))
        {
            if (_hasWeaponAttached)
                currentWeapon.Decomission();
           
            WeaponObject = Instantiate(powerUp.weaponPrefab, gameObject.transform);
            currentWeapon = WeaponObject.GetComponent<Weapon>();
            if (!currentWeapon)
                Debug.LogWarning($"{this.name} loaded a prefab from {other.gameObject.name}, but the prefab had no weapon component!");

            _hasWeaponAttached = true;
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(_targetVector, Vector3.up),
            rotateSpeed * Time.deltaTime);
    }
    
    public void Rotate(Vector3 input)
    {
        if (input == Vector3.zero)
            _targetVector = transform.forward;
        else
            _targetVector = SelectFacingVector(input);
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
