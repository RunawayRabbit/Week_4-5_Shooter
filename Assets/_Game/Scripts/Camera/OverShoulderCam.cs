
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : ICameraBehaviour
{
    private List<GameObject> _followTargets;
    private List<GameObject> _lookTargets;

    public CameraAttributes Attribs { get; }

    public OverShoulderCam(ref CameraAttributes attribs, GameObject followTarget = null, GameObject lookTarget = null)
    {
        _followTargets = new List<GameObject> {followTarget};
        _lookTargets = new List<GameObject> {lookTarget};

        Attribs = attribs;
    }
    
    public OverShoulderCam(ref CameraAttributes attribs, List<GameObject> followTargets = null, List<GameObject> lookTargets = null)
    {
        _followTargets = followTargets;
        _lookTargets = lookTargets;
        
        Attribs = attribs;
    }
    
    public void MoveCamera(CameraController cam, ref Vector3 position, ref Vector3 lookAt, out Vector3 cameraUp, ref float fov)
    {
        var camTransform = cam.transform;
        
        Vector3 newPos = AggregateTargets(_followTargets) + Attribs.displacement;
        var velocity = newPos - position;
        position = newPos;
        
        lookAt = AggregateTargets(_lookTargets) - camTransform.localPosition;

        var camTransformRight = camTransform.right;
        float lateralVelocity = Vector3.Dot(velocity, camTransformRight);
        //Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * Attribs.tiltAmount, camTransform.forward);
        var tiltedVector = camTransformRight * (lateralVelocity * Attribs.tiltAmount);
        cameraUp = (Vector3.up + tiltedVector).normalized;
        
        Debug.Log(cameraUp);
        fov = Attribs.FoV;
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