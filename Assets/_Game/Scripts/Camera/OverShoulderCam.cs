﻿
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : ICameraBehaviour
{
    private List<GameObject> _followTargets;
    private List<GameObject> _lookTargets;

    private readonly CameraAttributes _attribs;
    
    private Vector3 _velocity = Vector3.zero;

    public OverShoulderCam(CameraAttributes attribs, GameObject followTarget = null, GameObject lookTarget = null)
    {
        _followTargets = new List<GameObject> {followTarget};
        _lookTargets = new List<GameObject> {lookTarget};

        _attribs = attribs;
    }
    
    public OverShoulderCam(CameraAttributes attribs, List<GameObject> followTargets = null, List<GameObject> lookTargets = null)
    {
        _followTargets = followTargets;
        _lookTargets = lookTargets;
        
        _attribs = attribs;
    }
    
    public Vector3 GetPosition(CameraController cam)
    {
        Vector3 followTarget = AggregateTargets(_followTargets);
    
        Vector3 newPosition = followTarget + _attribs.displacement;
        return Vector3.SmoothDamp(cam.transform.position, newPosition, ref _velocity, _attribs.smoothTime);
    }

    public Quaternion GetRotation(CameraController cam)
    {
        Vector3 lookPosition = AggregateTargets(_lookTargets);
        float lateralVelocity = Vector3.Dot(_velocity, cam.transform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * _attribs.tiltAmount, Vector3.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - cam.transform.localPosition);

        if (_attribs.rotateSpeed <= 0.0f)
        {
            return newRotation;
        }        
        
        return Quaternion.RotateTowards(cam.transform.rotation, newRotation, _attribs.rotateSpeed);
    }

    public float GetFoV(CameraController cam, float currentFoV)
    {
        return _attribs.FoV;
    }
    
    private Vector3 AggregateTargets(List<GameObject> targets)
    {
        if (targets.Count == 1)
            return targets[0].transform.localPosition;

        var bounds = new Bounds();
        foreach (var target in targets)
            bounds.Encapsulate(target.transform.localPosition);

        return bounds.center;
    }
}