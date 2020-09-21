using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowCam : ICameraBehaviour
{
    public CamAttributes Attribs { get; set; }
    
    protected Vector3 _velocity = Vector3.zero;
    protected Vector3 _lookVelocity = Vector3.zero;
    protected Vector3 prevLookPosition;
    protected float _fovVelocity = 0.0f;
    
    public FollowCam(ref CamAttributes camAttribs)
    {
        Attribs = camAttribs;
        prevLookPosition = AggregateTargets(Attribs.lookTargets);
    }
   
    public Quaternion GetRotation(Transform inTransform)
    {
        Vector3 lookPosition = Vector3.SmoothDamp(prevLookPosition, AggregateTargets(Attribs.lookTargets), ref _lookVelocity,
            Attribs.smoothTime);

        float lateralVelocity = Vector3.Dot(_velocity, inTransform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * Attribs.tiltAmount, inTransform.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - inTransform.position, Attribs.cameraUp);

        prevLookPosition = lookPosition;
        
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
            return targets[0].transform.position;

        var bounds =  new Bounds(targets[0].transform.position, Vector3.zero);
        
        foreach (var target in targets)
        {
            bounds.Encapsulate(target.transform.position);
            Debug.Log(target.name);
        }

        Debug.DrawLine(bounds.min, bounds.max);
        
        return bounds.center;
    }
}
