using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour
{
    private Vector3 displacement = default;
    private float rotateSpeed = 3.0f;
    private float smoothTime = 0.1f;
    private float tiltAmount = 3.0f;

    private List<GameObject> _followTargets;
    private List<GameObject> _lookTargets;
    private Vector3 _velocity;

    public CameraBehaviour(CamBehaviourData camData, GameObject followTarget, GameObject lookTarget)
    {
        displacement = camData.displacement;
        rotateSpeed = camData.rotateSpeed;
        smoothTime = camData.smoothTime;
        tiltAmount = camData.tiltAmount;
        
        _followTargets = new List<GameObject>();
        _lookTargets = new List<GameObject>();
        
        _followTargets.Add(followTarget);
        _lookTargets.Add(lookTarget);
    }
    public CameraBehaviour(CamBehaviourData camData, List<GameObject> followTargets, List<GameObject> lookTargets)
    {
        displacement = camData.displacement;
        rotateSpeed = camData.rotateSpeed;
        smoothTime = camData.smoothTime;
        tiltAmount = camData.tiltAmount;

        _followTargets = _followTargets;
        _lookTargets = _lookTargets;
    }
    public virtual Vector3 GetPosition(Vector3 inPosition)
    {
        Vector3 followTarget = AggregateTargets(_followTargets);
        
        Vector3 newPosition = followTarget + displacement;
        return Vector3.SmoothDamp(inPosition, newPosition, ref _velocity, smoothTime);
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

    public Quaternion GetRotation(Transform transform)
    {
        Vector3 lookPosition = AggregateTargets(_lookTargets);
        
        float lateralVelocity = Vector3.Dot(_velocity, transform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * tiltAmount, Vector3.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - transform.localPosition);
        return Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed);
    }
}