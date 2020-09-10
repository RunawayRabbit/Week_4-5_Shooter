﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] GameObject target = default;
    [SerializeField] private float _distanceFromTarget = 3.0f;
    [SerializeField] private float _moveSpeed = 4.0f;
    
    private Arena _arena = default;
    private Vector3 _velocity = default;

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

        var desiredPosition = target.transform.localPosition - Vector3.back * _distanceFromTarget;

        float smoothTime = _arena.CurrentMode == Arena.Mode.Horizontal ? 0.01f : 0.6f;
        
        Vector3 newPosition = Vector3.SmoothDamp(currentTransform.position, desiredPosition, ref _velocity, smoothTime, _moveSpeed);

        currentTransform.position = newPosition;
        currentTransform.localRotation = look;
    }
}
