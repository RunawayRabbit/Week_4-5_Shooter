using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : ICameraBehaviour
{
    private List<GameObject> _followTargets;

    public CameraAttributes Attribs { get; set; }

    public TopDownCam(ref CameraAttributes attribs, GameObject followTarget = null, GameObject lookTarget = null)
    {
        _followTargets = new List<GameObject> {followTarget};
        Attribs = attribs;
    }
    
    public TopDownCam(ref CameraAttributes attribs, List<GameObject> followTargets = null, GameObject lookTarget = null)
    {
        _followTargets = followTargets;
        Attribs = attribs;
    }
    
    public void MoveCamera(CameraController cam, ref Vector3 position, ref Vector3 lookAt, out Vector3 cameraUp, ref float fov)
    {
        position = AggregateTargets(_followTargets) + Attribs.displacement;
        lookAt = Vector3.down;
        cameraUp = Vector3.forward;
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