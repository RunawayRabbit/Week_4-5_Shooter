
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

    public void StartShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StartShooting();
    }

    public void StopShooting()
    {
        if (_hasWeaponAttached) currentWeapon.StopShooting();
    }

    private void Awake()
    {
        PowerUpLayer = LayerMask.NameToLayer("Powerup");
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

    public void Rotate(Vector3 input)
    {
        if (input == Vector3.zero)
        {
            _targetVector = transform.forward;
        }
        else
        {
            _targetVector = SelectFacingVector(input);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(_targetVector, Vector3.up),
            rotateSpeed * Time.deltaTime);
    }

    public Vector3 SelectFacingVector(Vector3 inVector)
    {
        //@NOTE: We assume our velocity is constrained to the X/Z plane.
        // 1 minRotation, 2 velocity, 3 maxRotation
        // Determine winding direction of our 3 points.
        var vector = -inVector;
        float slopeFactor = (vector.z - minRotation.z) * (maxRotation.x - vector.x) -
                            (maxRotation.z - vector.z) * (vector.x - minRotation.x);

        if (slopeFactor == 0.0f) return _targetVector;

        return slopeFactor < 0.0f ? minRotation : maxRotation;
    }
}
