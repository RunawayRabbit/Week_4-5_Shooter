using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : ICameraBehaviour
{
    private List<GameObject> _followTargets;
    private List<GameObject> _lookTargets;

    private CameraAttributes _attribs;

    private Vector3 _velocity = Vector3.zero;

    public TopDownCam(CameraAttributes attribs, GameObject followTarget = null, GameObject lookTarget = null)
    {
        _followTargets = new List<GameObject>();
        _followTargets.Add(followTarget);
        _lookTargets = new List<GameObject>();
        _lookTargets.Add(lookTarget);

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
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - cam.transform.localPosition, Vector3.forward);
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