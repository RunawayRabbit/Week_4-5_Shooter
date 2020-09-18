using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : ICameraBehaviour
{
    private List<GameObject> _followTargets;
    private List<GameObject> _lookTargets;

    private CamAttributes _attribs;
    
    protected Vector3 _velocity;
    
    public OverShoulderCam(ref CamAttributes camAttribs, GameObject followTarget, GameObject lookTarget)
    {
        _attribs = camAttribs;
        _followTargets = new List<GameObject> {followTarget};
        _lookTargets = new List<GameObject> {lookTarget};
    }
   
    public Quaternion GetRotation(Transform inTransform)
    {
        Vector3 lookPosition = AggregateTargets(_lookTargets);
        
        float lateralVelocity = Vector3.Dot(_velocity, inTransform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * _attribs.tiltAmount, Vector3.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - inTransform.localPosition);
        return Quaternion.RotateTowards(inTransform.rotation, newRotation, _attribs.rotateSpeed);
    }

    public float GetFoV(float currentFoV)
    {
        return 50.0f;
    }

    public Vector3 GetPosition(Vector3 inPosition)
    {
        Vector3 followTarget = AggregateTargets(_followTargets);
    
        Vector3 newPosition = followTarget + _attribs.displacement;
        return Vector3.SmoothDamp(inPosition, newPosition, ref _velocity, _attribs.smoothTime);
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
