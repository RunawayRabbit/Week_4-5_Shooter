using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
[SelectionBase]
public class PlayerWeaponSlot : WeaponSlot
{
    private float _arcWindingDirection = default;
    private Arena.Mode _currentMode = Arena.Mode.Horizontal;
    private SphereCollider _ourCollider;
    private GameObject _targetReticule = default;
    private Vector3 _targetVector = Vector3.forward;

    // @TODO: Really think about weapon destruction! Does it make the game better? Is it worth pursuing?
    //[SerializeField] private float colliderRadiusWhileEmpty = 2.0f;
    [SerializeField] private float colliderRadiusWhileActive = 1.0f;
    [HideInInspector] public Vector3 maxRotation;
    [HideInInspector] public Vector3 minRotation = Vector3.forward;

    [SerializeField] private float rotateSpeed = 50.0f;

    [SerializeField] private InputActionReference shootReference = default;

    [SerializeField] public float turningArc;

    private void OnEnable()
    {
        Arena.Instance.OnModeChange += ChangeMode;
    }

    private void OnDisable()
    {
        Arena.Instance.OnModeChange -= ChangeMode;
    }

    private void ChangeMode(Arena.Mode newMode)
    {
        _currentMode = newMode;
    }

    protected override void Awake()
    {
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);

        _targetReticule = GameObject.FindWithTag("TargetReticule");
        if (!_targetReticule) Debug.LogWarning("Target Reticle not found! Remember to set the TAG for it!");

        _ourCollider = GetComponent<SphereCollider>();
        if (!_ourCollider)
            Debug.LogWarning($"{name} has no SphereCollider on it. We can't pick up powerups without one!");

        Debug.Assert(shootReference,
            $"{name} has no shootReference set. Make sure you have selected the \"shoot\" action input in the inspector.");
        // Start shooting immediately if the button is down.
        if (shootReference.action.phase == InputActionPhase.Started)
            StartShooting();

        base.Awake();
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

    public override void EquipWeapon(GameObject weaponPrefab)
    {
        _ourCollider.radius = colliderRadiusWhileActive;
        base.EquipWeapon(weaponPrefab);
    }

    public void Rotate(Vector3 input)
    {
        if (_currentMode == Arena.Mode.Horizontal)
        {
            if (rotateSpeed <= 0.0f) return;
            _targetVector = input == Vector3.zero ? transform.forward : SelectFacingVector(input);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _arcWindingDirection = Vector3.Dot(Vector3.Cross(minRotation, maxRotation), Vector3.up);
    }
#endif

    private Vector3 SelectFacingVector(Vector3 input)
    {
        var minToInputWinding = Vector3.Dot(Vector3.Cross(minRotation, input), Vector3.up);
        var maxToInputWinding = Vector3.Dot(Vector3.Cross(maxRotation, input), Vector3.up);

        var isMinToInputSameWinding = Mathf.Sign(_arcWindingDirection) == Math.Sign(minToInputWinding);
        var isMaxToInputSameWinding = Mathf.Sign(_arcWindingDirection) == Math.Sign(maxToInputWinding);

        if (!isMinToInputSameWinding && !isMaxToInputSameWinding) return maxRotation;
        return minRotation;
    }

    public void StopRotation()
    {
        _targetVector = transform.forward;
    }
}