using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : ICameraBehaviour
{
    public CamAttributes Attribs { get; set; }
    
    protected Vector3 _velocity = Vector3.zero;
    protected float _fovVelocity = 0.0f;
    
    public OverShoulderCam(ref CamAttributes camAttribs)
    {
        Attribs = camAttribs;
    }
   
    public Quaternion GetRotation(Transform inTransform)
    {
        Vector3 lookPosition = AggregateTargets(Attribs.lookTargets);
        
        float lateralVelocity = Vector3.Dot(_velocity, inTransform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * Attribs.tiltAmount, Vector3.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - inTransform.localPosition);
        return Quaternion.RotateTowards(inTransform.rotation, newRotation, Attribs.rotateSpeed);
    }

    public float GetFoV(float currentFoV)
    {
        return Mathf.SmoothDamp(currentFoV, Attribs.fieldOfView, ref _fovVelocity, Attribs.fieldOfViewSmoothTime);
    }

    public Vector3 GetPosition(Vector3 inPosition)
    {
        Vector3 followTarget = AggregateTargets(Attribs.followTargets);
    
        Vector3 newPosition = followTarget + Attribs.displacement;
        return Vector3.SmoothDamp(inPosition, newPosition, ref _velocity, Attribs.smoothTime);
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
